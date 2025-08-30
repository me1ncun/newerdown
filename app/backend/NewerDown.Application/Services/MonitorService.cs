using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Application.Constants;
using NewerDown.Application.Extensions;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;
using Monitor = NewerDown.Domain.Entities.Monitor;

namespace NewerDown.Application.Services;

public class MonitorService : IMonitorService
{
    private const string CacheKey = nameof(Monitor);

    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly IUserContextService _userContextService;
    private readonly IHttpClientFactory _httpClientFactory;

    public MonitorService(
        ApplicationDbContext context,
        IMapper mapper,
        ICacheService cacheService,
        IUserContextService userContextService,
        IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
        _userContextService = userContextService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<MonitorDto>> GetAllMonitors()
    {
        var cached = await _cacheService.GetAsync<IEnumerable<MonitorDto>>(CacheKey);
        if (cached is not null)
            return cached;

        var monitors = await _context.Monitors
            .Where(x => x.UserId == _userContextService.GetUserId())
            .ToListAsync();

        var result = _mapper.Map<List<MonitorDto>>(monitors);
        await _cacheService.SetAsync(CacheKey, result, TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes));

        return result;
    }

    public async Task<Guid> CreateMonitorAsync(AddMonitorDto monitorDto)
    {
        var monitorExists = await GetServiceByNameAsync(monitorDto.Name);

        var monitor = _mapper.Map<Monitor>(monitorDto);
        monitor.UserId = _userContextService.GetUserId();
        monitor.Id = Guid.NewGuid();
        
        if (!await IsServiceSiteValid(monitor))
            throw new HttpRequestException("The provided URL is not reachable or valid.");

        _context.Monitors.Add(monitor);
        await _context.SaveChangesAsync();

        return monitor.Id;
    }

    public async Task UpdateMonitorAsync(Guid serviceId, UpdateMonitorDto monitorDto)
    {
        var monitor = await GetMonitorByIdAsync(serviceId);

        monitor.UserId = _userContextService.GetUserId();

        _mapper.Map(monitorDto, monitor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMonitorAsync(Guid id)
    {
        var monitorDto = await GetMonitorByIdAsync(id);
        
        var monitor = _mapper.Map<Monitor>(monitorDto);

        _context.Monitors.Remove(monitor);
        await _context.SaveChangesAsync();
    }

    public async Task<MonitorDto> GetMonitorByIdAsync(Guid id)
    {
        var monitor = (await _context.Monitors
            .FirstOrDefaultAsync(s => s.Id == id)).ThrowIfNull(nameof(Monitor));

        return _mapper.Map<MonitorDto>(monitor);
    }

    private async Task<bool> IsServiceSiteValid(Monitor monitor)
    {
        var client = _httpClientFactory.CreateClient();
        var result = await client.GetAsync(monitor.Target);
        return result.IsSuccessStatusCode;
    }

    private async Task<MonitorDto> GetServiceByNameAsync(string name)
    {
        var monitor = await _context.Monitors
            .FirstOrDefaultAsync(s => s.UserId == _userContextService.GetUserId()
                                      && s.Name == name);

        return _mapper.Map<MonitorDto>(monitor);
    }
}