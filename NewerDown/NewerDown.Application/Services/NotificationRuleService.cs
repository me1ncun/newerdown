using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Application.Constants;
using NewerDown.Domain.DTOs.Notifications;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class NotificationRuleService : INotificationRuleService
{
    private const string CacheKey = "AllNotificationRules";
    
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    
    public NotificationRuleService(
        ApplicationDbContext context,
        IMapper mapper,
        ICacheService cacheService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<NotificationRuleDto>> GetAllAsync()
    {
        var cached = await _cacheService.GetAsync<IEnumerable<NotificationRuleDto>>(CacheKey);
        if (cached is not null)
            return cached;
        
        var notificationRules = await _context.NotificationRules.ToListAsync();
        var result = _mapper.Map<IEnumerable<NotificationRuleDto>>(notificationRules);
        await _cacheService.SetAsync(CacheKey, result, TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes));
        
        return result;
    }
    
    public async Task CreateNotificationRuleAsync(AddNotificationRuleDto notificationRuleDto)
    {
        var notificationRule = await _context.NotificationRules
            .FirstOrDefaultAsync(nr => nr.ServiceId == notificationRuleDto.ServiceId 
                                       && nr.UserId == notificationRuleDto.UserId);
        
        if (notificationRule is not null)
            throw new EntityAlreadyExistsException(nameof(NotificationRule));
        
        _context.NotificationRules.Add(_mapper.Map<NotificationRule>(notificationRuleDto));
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNotificationRuleAsync(Guid id)
    {
        var notificationRule = await GetNotificationRuleByIdAsync(id);
        _context.NotificationRules.Remove(notificationRule);
        await _context.SaveChangesAsync();
    }
    
    private async Task<NotificationRule> GetNotificationRuleByIdAsync(Guid id)
    {
        var notificationRule = await _context.NotificationRules
            .FirstOrDefaultAsync(nr => nr.Id == id);
        
        if (notificationRule is null)
            throw new EntityNotFoundException(nameof(NotificationRule));
        
        return notificationRule;
    }
}