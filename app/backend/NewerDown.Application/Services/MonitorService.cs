using System.Globalization;
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewerDown.Application.Constants;
using NewerDown.Application.CsvProfiles;
using NewerDown.Application.Errors;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Interfaces;
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

    public MonitorService(
        ApplicationDbContext context,
        IMapper mapper,
        ICacheService cacheService,
        IUserContextService userContextService,
        IHttpClientFactory httpClientFactory,
        ILogger<MonitorService> logger)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
        _userContextService = userContextService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _cacheKey = $"{nameof(Monitor)}_{_userContextService.GetUserId()}";
    }

    public async Task<IEnumerable<MonitorDto>> GetAllMonitors()
    {
        var cached = await _cacheService.GetAsync<IEnumerable<MonitorDto>>(_cacheKey);
        if (cached is not null)
            return cached;

        var monitors = await _context.Monitors
            .Where(x => x.UserId == _userContextService.GetUserId())
            .ToListAsync();

        var result = _mapper.Map<List<MonitorDto>>(monitors);
        await _cacheService.SetAsync(_cacheKey, result, TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes));

        return result;
    }

    public async Task<Result<Guid>> CreateMonitorAsync(AddMonitorDto monitorDto)
    {
        await GetServiceByNameAsync(monitorDto.Name);

        var monitor = _mapper.Map<Monitor>(monitorDto);
        monitor.UserId = _userContextService.GetUserId();
        monitor.Id = Guid.NewGuid();
        
        if (!await IsServiceSiteValid(monitor.Target))
            return Result<Guid>.Failure(MonitorErrors.TargetNotReachable);

        _context.Monitors.Add(monitor);
        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);

        return Result<Guid>.Success(monitor.Id);
    }

    public async Task<Result> UpdateMonitorAsync(Guid monitorId, UpdateMonitorDto monitorDto)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(x => x.Id == monitorId
                                                                       && x.UserId == _userContextService.GetUserId());
        if (monitor is null)
            return Result.Failure(MonitorErrors.MonitorNotFound);
        
        monitor.UserId = _userContextService.GetUserId();
        _mapper.Map(monitorDto, monitor);
        if (!await IsServiceSiteValid(monitorDto.Url))
            return Result.Failure(MonitorErrors.TargetNotReachable);
        
        _context.Monitors.Update(monitor);
        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);
        
        return Result.Success();
    }

    public async Task<Result> DeleteMonitorAsync(Guid id)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(x => x.Id == id
                                                                       && x.UserId == _userContextService.GetUserId());
        
        if (monitor is null)
            return Result.Failure(MonitorErrors.MonitorNotFound);
        
        _context.Monitors.Remove(monitor);
        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);
        
        return Result.Success();
    }

    public async Task<Result<MonitorDto>> GetMonitorByIdAsync(Guid id)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(s => s.Id == id);
        if (monitor is null || monitor.UserId != _userContextService.GetUserId())
            return Result<MonitorDto>.Failure(MonitorErrors.MonitorNotFound);

        return Result<MonitorDto>.Success(_mapper.Map<MonitorDto>(monitor));
    }
    
    public async Task<Result> PauseMonitorAsync(Guid id)
    {
        var monitor = await _context.Monitors.FirstOrDefaultAsync(x => x.Id == id 
                                                                       && x.UserId == _userContextService.GetUserId());
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
        var monitor = await _context.Monitors.FirstOrDefaultAsync(x => x.Id == id 
                                                                       && x.UserId == _userContextService.GetUserId());
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
        var monitor = _context.Monitors.FirstOrDefault(x => x.Id == id 
                                                        && x.UserId == _userContextService.GetUserId());
 
        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream);
        await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
      
        csvWriter.Context.RegisterClassMap<MonitorDtoProfile>();
        await csvWriter.WriteRecordsAsync(new List<MonitorDto> { _mapper.Map<MonitorDto>(monitor) });
        
        await streamWriter.FlushAsync();
 
        _logger.LogInformation("Generating CSV SUCCEEDED");
 
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
                .FirstOrDefaultAsync(s => s.UserId == _userContextService.GetUserId()
                                          && s.Name == record.Name);
            if (existingMonitor is not null)
                continue;

            var monitor = _mapper.Map<Monitor>(record);
            monitor.UserId = _userContextService.GetUserId();
            monitor.Id = Guid.NewGuid();
            
            if (!await IsServiceSiteValid(monitor.Target))
                continue;

            _context.Monitors.Add(monitor);
        }

        await _context.SaveChangesAsync();
        await _cacheService.RemoveAsync(_cacheKey);
    }

    private async Task<bool> IsServiceSiteValid(string target)
    {
        var client = _httpClientFactory.CreateClient();
        var result = await client.GetAsync(target);
        return result.IsSuccessStatusCode;
    }

    private async Task<Result<MonitorDto>> GetServiceByNameAsync(string name)
    {
        var monitor = await _context.Monitors
            .FirstOrDefaultAsync(s => s.UserId == _userContextService.GetUserId()
                                      && s.Name == name);
        
        if (monitor is null)
            return Result<MonitorDto>.Failure(MonitorErrors.MonitorNotFound);

        return Result<MonitorDto>.Success(_mapper.Map<MonitorDto>(monitor));
    }
}