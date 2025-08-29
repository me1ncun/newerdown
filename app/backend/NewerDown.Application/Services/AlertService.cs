using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Application.Constants;
using NewerDown.Application.Extensions;
using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class AlertService : IAlertService
{
    private const string CacheKey = nameof(Alert);
    
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly IUserService _userService;
    
    public AlertService(
        ApplicationDbContext context,
        IMapper mapper,
        ICacheService cacheService,
        IUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
        _userService = userService;
    }

    public async Task<IEnumerable<AlertDto>> GetAllAsync()
    {
        var cached = await _cacheService.GetAsync<IEnumerable<AlertDto>>(CacheKey);
        if (cached is not null)
            return cached;
        
        var alerts = await _context.Alerts
            .Where(x => x.MonitorId == _userService.GetUserId())
            .ToListAsync();
        
        var result = _mapper.Map<List<AlertDto>>(alerts);
        await _cacheService.SetAsync(CacheKey, result, TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes));
        
        return result;
    }

    public Task<AlertDto> UpdateAlertAsync(Guid id, UpdateAlertDto updateAlertDto)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAlertAsync(AddAlertDto alertDto)
    {
        var currentUserId = _userService.GetUserId();
        var exists = await _context.Alerts.FirstOrDefaultAsync(a => a.MonitorId == currentUserId);

        if (exists is not null)
            throw new EntityAlreadyExistsException();

        var alert = _mapper.Map<Alert>(alertDto);
        alert.Id = Guid.NewGuid();
        alert.MonitorId = currentUserId;
        
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAlertAsync(Guid id)
    {
        var alert = await _context.Alerts.FirstOrDefaultAsync(a => a.Id == id);
        if (alert is null)
            throw new EntityNotFoundException(nameof(Alert));
        
        _context.Alerts.Remove(alert);
        await _context.SaveChangesAsync();
    }
    
    public async Task<AlertDto> GetAlertByIdAsync(Guid id)
    {
        var alert = (await _context.Alerts
            .FirstOrDefaultAsync(nr => nr.Id == id)).ThrowIfNull(nameof(Alert));
        
        return _mapper.Map<AlertDto>(alert);
    }
}