using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NewerDown.Application.Constants;
using NewerDown.Application.Extensions;
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
    private readonly IUserService _userService;
    
    public NotificationRuleService(
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

    public async Task<IEnumerable<NotificationRuleDto>> GetAllAsync()
    {
        var cached = await _cacheService.GetAsync<IEnumerable<NotificationRuleDto>>(CacheKey);
        if (cached is not null)
            return cached;
        
        var notificationRules = await _context.NotificationRules
            .Where(x => x.UserId == _userService.GetUserId())
            .ToListAsync();
        
        var result = _mapper.Map<List<NotificationRuleDto>>(notificationRules);
        await _cacheService.SetAsync(CacheKey, result, TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes));
        
        return result;
    }
    
    public async Task CreateNotificationRuleAsync(AddNotificationRuleDto notificationRuleDto)
    {
        var currentUserId = _userService.GetUserId();
        var exists = await _context.NotificationRules.FirstOrDefaultAsync(nr => nr.ServiceId == notificationRuleDto.ServiceId 
                                                                    && nr.UserId == currentUserId);

        if (exists is not null)
            throw new EntityAlreadyExistsException();

        var notificationRule = _mapper.Map<NotificationRule>(notificationRuleDto);
        notificationRule.Id = Guid.NewGuid();
        notificationRule.UserId = currentUserId;
        
        _context.NotificationRules.Add(notificationRule);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNotificationRuleAsync(Guid id)
    {
        var notificationRule = await GetNotificationRuleByIdAsync(id);
        
        _context.NotificationRules.Remove(notificationRule);
        await _context.SaveChangesAsync();
    }
    
    public async Task<NotificationRule> GetNotificationRuleByIdAsync(Guid id)
    {
        var notificationRule = (await _context.NotificationRules
            .FirstOrDefaultAsync(nr => nr.Id == id)).ThrowIfNull(nameof(NotificationRule));
        
        return notificationRule;
    }
}