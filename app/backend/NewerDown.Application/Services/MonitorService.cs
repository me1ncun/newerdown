using System.Globalization;
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewerDown.Application.Constants;
using NewerDown.Application.CsvProfiles;
using NewerDown.Application.Errors;
using NewerDown.Application.Time;
using NewerDown.Domain.DTOs.MonitorCheck;
using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Domain.Paging;
using NewerDown.Domain.Result;
using NewerDown.Infrastructure.Data;
using Monitor = NewerDown.Domain.Entities.Monitor;

namespace NewerDown.Application.Services;

public class MonitorService : IMonitorService
{
    private readonly string _cacheKey;
    
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly IUserContextService _userContextService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MonitorService> _logger;
    private readonly IScopedTimeProvider _timeProvider;

    public MonitorService(
        ApplicationDbContext context,
        IMapper mapper,
        ICacheService cacheService,
        IUserContextService userContextService,
        IHttpClientFactory httpClientFactory,
        ILogger<MonitorService> logger,
        IScopedTimeProvider timeProvider)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
        _userContextService = userContextService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _cacheKey = $"{nameof(Monitor)}_{_userContextService.GetUserId()}";
        _timeProvider = timeProvider;
    }

    public async Task<IEnumerable<MonitorDto>> GetAllMonitors()
    {
        var cached = await _cacheService.GetAsync<IEnumerable<MonitorDto>>(_cacheKey);
        if (cached is not null)
            return cached;

        var monitors = await _context.Monitors
            .Where(m => m.UserId == _userContextService.GetUserId())
            .ToListAsync();

        var result = _mapper.Map<List<MonitorDto>>(monitors);
        await _cacheService.SetAsync(_cacheKey, result, TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes));

        return result;
    }

    public async Task<Result<Guid>> CreateMonitorAsync(AddMonitorDto monitorDto)
    {
        var existingMonitor = await _context.Monitors.FirstOrDefaultAsync(m => m.UserId == _userContextService.GetUserId()
                                                                          && m.Name == monitorDto.Name);

        if (existingMonitor != null)
            throw new EntityAlreadyExistsException($"Monitor with name {monitorDto.Name} already exists");

        var monitor = _mapper.Map<Monitor>(monitorDto);
        monitor.UserId = _userContextService.GetUserId();
        monitor.Id = Guid.NewGuid();
        monitor.CreatedAt = _timeProvider.UtcNow();

        if (!await IsTargetReachable(monitor.Target))
            return Result<Guid>.Failure(MonitorErrors.TargetNotReachable);

        _context.Monitors.Add(monitor);
        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);

        return Result<Guid>.Success(monitor.Id);
    }

    public async Task<Result> UpdateMonitorAsync(Guid monitorId, UpdateMonitorDto monitorDto)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(m => m.Id == monitorId
                                                                       && m.UserId == _userContextService.GetUserId());
        if (monitor is null)
            return Result.Failure(MonitorErrors.MonitorNotFound);
        
        monitor.UserId = _userContextService.GetUserId();
        _mapper.Map(monitorDto, monitor);

        if (!await IsTargetReachable(monitorDto.Url))
            return Result.Failure(MonitorErrors.TargetNotReachable);
        
        _context.Monitors.Update(monitor);
        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);
        
        return Result.Success();
    }

    public async Task<Result> DeleteMonitorAsync(Guid id)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userContextService.GetUserId());
        
        if (monitor is null)
            return Result.Failure(MonitorErrors.MonitorNotFound);
        
        _context.Monitors.Remove(monitor);
        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);
        
        return Result.Success();
    }

    public async Task<Result<MonitorDto>> GetMonitorByIdAsync(Guid id)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(m => m.Id == id);
        if (monitor is null || monitor.UserId != _userContextService.GetUserId())
            return Result<MonitorDto>.Failure(MonitorErrors.MonitorNotFound);

        return Result<MonitorDto>.Success(_mapper.Map<MonitorDto>(monitor));
    }
    
    public async Task<Result> PauseMonitorAsync(Guid id)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userContextService.GetUserId());
        if (monitor is null)
            return Result<MonitorDto>.Failure(MonitorErrors.MonitorNotFound);

        monitor.IsActive = false;
        _context.Monitors.Update(monitor);

        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);
        
        return Result.Success();
    }
    
    public async Task<Result> ResumeMonitorAsync(Guid id)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userContextService.GetUserId());
        if (monitor is null)
            return Result<MonitorDto>.Failure(MonitorErrors.MonitorNotFound);

        monitor.IsActive = true;
        _context.Monitors.Update(monitor);
        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);
        
        return Result.Success();
    }

    public async Task<byte[]> ExportMonitorCsvAsync(Guid id)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userContextService.GetUserId());
        if (monitor is null)
            throw new EntityNotFoundException($"Monitor with id: {id} not found");

        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream);
        await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
      
        csvWriter.Context.RegisterClassMap<MonitorDtoProfile>();
        await csvWriter.WriteRecordsAsync(new List<MonitorDto> { _mapper.Map<MonitorDto>(monitor) });
        
        await streamWriter.FlushAsync();
 
        _logger.LogInformation("Generating CSV succeeded");
 
        return memoryStream.ToArray();
    }
    
    public async Task ImportMonitorsFromCsvAsync(IFormFile file)
    {
        await using var memoryStream = file.OpenReadStream();
        using var reader = new StreamReader(memoryStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<AddMonitorDtoProfile>();
        var records = csv.GetRecords<AddMonitorDto>().ToList();

        foreach (var record in records)
        {
            var existingMonitor = await _context.Monitors
                .FirstOrDefaultAsync(m => m.UserId == _userContextService.GetUserId()
                                          && m.Name == record.Name);
            if (existingMonitor is not null)
                continue;

            var monitor = _mapper.Map<Monitor>(record);
            monitor.UserId = _userContextService.GetUserId();
            monitor.Id = Guid.NewGuid();
            
            if (!await IsTargetReachable(monitor.Target))
                continue;

            _context.Monitors.Add(monitor);
        }

        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);
    }
    
    public async Task<MonitorStatus> GetMonitorStatusAsync(Guid id)
    {
        var monitor = await _context.Monitors
            .Include(m => m.Checks.OrderByDescending(mc => mc.CheckedAt).Take(1))
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userContextService.GetUserId());

        if (monitor is null)
            throw new EntityNotFoundException($"No checks found for monitor with id: {id}");

        var lastCheck = monitor.Checks.FirstOrDefault();
        if (lastCheck is null)
            throw new EntityNotFoundException($"Monitor with id: {id} has no check history yet");

        return lastCheck.IsSuccess ? MonitorStatus.Up : MonitorStatus.Down;
    }
    
    public async Task<PagedList<MonitorCheckDto>> GetHistoryByMonitorAsync(Guid id, int pageNumber = 1, int pageSize = 30)
    {
        var query = _context.MonitorChecks
            .Where(mc => mc.MonitorId == id && mc.Monitor.UserId == _userContextService.GetUserId())
            .OrderByDescending(mc => mc.CheckedAt);
        
        var pagedList = await PagedList<MonitorCheck>.CreateAsync(query, pageNumber, pageSize);

        return _mapper.Map<PagedList<MonitorCheckDto>>(pagedList);
    }
    
    public async Task<UptimePercentageDto> GetUptimePercentageAsync(Guid id, DateTime from, DateTime to)
    {
        var monitor = await _context.Monitors
            .Include(m => m.Checks.Where(mc => mc.CheckedAt >= from && mc.CheckedAt <= to))
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == _userContextService.GetUserId());

        if (monitor is null)
            throw new EntityNotFoundException($"No checks found for monitor with id: {id}");

        var totalChecks = monitor.Checks.Count;
        if (totalChecks == 0)
            return new UptimePercentageDto { Percentage = 0 };

        var successfulChecks = monitor.Checks.Count(mc => mc.IsSuccess);
        var uptimePercentage = (successfulChecks / (double)totalChecks) * 100;

        return new UptimePercentageDto { Percentage = Math.Round(uptimePercentage, 2) };
    }

    public async Task<List<MonitorCheckShortDto>> GetLatencyGraph(Guid id, DateTime from, DateTime to)
    {
        var points = await _context.MonitorChecks
            .Where(c => c.MonitorId == id && c.CheckedAt >= from && c.CheckedAt <= to && c.ResponseTimeMs != null)
            .OrderBy(c => c.CheckedAt)
            .Select(c => new MonitorCheckShortDto()
            {
                CheckedAt = c.CheckedAt,
                ResponseTimeMs = c.ResponseTimeMs
            })
            .ToListAsync();

        return points;
    }

    public async Task<List<DownTimeDto>> GetDownTimesAsync(Guid id)
    {
        var checks = await _context.MonitorChecks
            .Where(c => c.MonitorId == id)
            .OrderBy(c => c.CheckedAt)
            .ToListAsync();

        var downtimes = new List<DownTimeDto>();
        DateTime? start = null;

        foreach (var check in checks)
        {
            if (!check.IsSuccess && start == null)
                start = check.CheckedAt;
            
            else if (check.IsSuccess && start != null)
            {
                downtimes.Add(new DownTimeDto() { Start = start, End = check.CheckedAt });
                start = null;
            }
        }
        
        if (start != null)
            downtimes.Add(new DownTimeDto() { Start = start, End = (DateTime?)null });

        return downtimes;
    }

    public async Task<MonitorSummaryDto> GetMonitorSummaryAsync(Guid id, int hours)
    {
        var stats = await _context.MonitorStatistics
            .Where(s => s.MonitorId == id)
            .OrderByDescending(s => s.PeriodEnd)
            .Take(hours)
            .ToListAsync();

        if (!stats.Any())
            throw new EntityNotFoundException($"Monitor statistic not found with monitor id {id}");

        var summary = new MonitorSummaryDto()
        {
            UptimePercent = stats.Average(s => s.UptimePercent),
            AvgResponseTimeMs = stats.Average(s => s.AvgResponseTimeMs),
            TotalChecks = stats.Sum(s => s.TotalChecks),
            FailedChecks = stats.Sum(s => s.FailedChecks),
            IncidentsCount = stats.Sum(s => s.IncidentsCount)
        };

        return summary;
    }

    private async Task<bool> IsTargetReachable(string target)
    {
        var client = _httpClientFactory.CreateClient();
        var result = await client.GetAsync(target);
        return result.IsSuccessStatusCode;
    }
}