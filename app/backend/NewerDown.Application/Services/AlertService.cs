using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NewerDown.Application.Constants;
using NewerDown.Domain.Builders;
using NewerDown.Domain.DTOs.Alerts;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class AlertService : IAlertService
{
    private readonly string _cacheKey;
    
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly IUserContextService _userContextService;
    
    public AlertService(
        ApplicationDbContext context,
        IMapper mapper,
        ICacheService cacheService,
        IUserContextService userContextService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
        _userContextService = userContextService;
        _cacheKey = $"{nameof(Alert)}_{userContextService.GetUserId()}";
    }

    public async Task<List<AlertDto>> GetAllAsync()
    {
        var cachedResult = await _cacheService.GetAsync<List<AlertDto>>(_cacheKey);
        if (cachedResult is not null)
            return cachedResult;
        
        var alerts = await _context.Alerts
            .Where(x => x.UserId == _userContextService.GetUserId())
            .ToListAsync();
        
        var result = _mapper.Map<List<AlertDto>>(alerts);
        await _cacheService.SetAsync(_cacheKey, result, TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes));
        
        return result;
    }

    public async Task UpdateAlertAsync(Guid id, UpdateAlertDto request)
    {
        var alert = await _context.Alerts.FirstOrDefaultAsync(a => a.Id == id);
        if(alert is null)
            throw new EntityNotFoundException(nameof(Alert));

        _mapper.Map(request, alert);
        _context.Alerts.Update(alert);
        await _context.SaveChangesAsync();
        
        await _cacheService.RemoveAsync(_cacheKey);
    }

    public async Task CreateAlertAsync(AddAlertDto request)
    {
        var currentUserId = _userContextService.GetUserId();
        var alertExists = await _context.Alerts.FirstOrDefaultAsync(a => a.MonitorId == currentUserId);
        if (alertExists is not null)
            throw new EntityAlreadyExistsException();

        var alert = _mapper.Map<Alert>(request);
        alert.Id = Guid.NewGuid();
        alert.UserId = currentUserId;
        
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();
        
        await _cacheService.RemoveAsync(_cacheKey);
    }

    public async Task DeleteAlertAsync(DeleteAlertDto request)
    {
        var alert = await _context.Alerts.FirstOrDefaultAsync(a => a.Id == request.Id);
        if (alert is null)
            throw new EntityNotFoundException($"Alert not found by Id: {request.Id}");
        
        _context.Alerts.Remove(alert);
        await _context.SaveChangesAsync();
        
        await _cacheService.RemoveAsync(_cacheKey);
    }

    public async Task<AlertDto> GetAlertByIdAsync(Guid id)
    {
        var alert = await _context.Alerts.FirstOrDefaultAsync(a => a.Id == id);
        if(alert is null)
            throw new EntityNotFoundException($"Alert not found by Id: {id}");
        
        return _mapper.Map<AlertDto>(alert);
    }
    
}