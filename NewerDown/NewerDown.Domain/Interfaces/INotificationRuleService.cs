using NewerDown.Domain.DTOs.Notifications;

namespace NewerDown.Domain.Interfaces;

public interface INotificationRuleService
{
    Task<IEnumerable<NotificationRuleDto>> GetAllAsync();
    Task CreateNotificationRuleAsync(AddNotificationRuleDto notificationRuleDto);
    Task DeleteNotificationRuleAsync(Guid id);
}