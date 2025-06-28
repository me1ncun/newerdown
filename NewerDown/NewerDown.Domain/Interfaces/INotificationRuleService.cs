using NewerDown.Domain.DTOs.Notifications;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface INotificationRuleService
{
    Task<IEnumerable<NotificationRuleDto>> GetAllAsync();
    Task CreateNotificationRuleAsync(AddNotificationRuleDto notificationRuleDto);
    Task DeleteNotificationRuleAsync(Guid id);
    Task<NotificationRule> GetNotificationRuleByIdAsync(Guid id);
}