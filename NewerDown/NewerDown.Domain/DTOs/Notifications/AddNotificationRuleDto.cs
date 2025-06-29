using NewerDown.Domain.Entities;

namespace NewerDown.Domain.DTOs.Notifications;

public class AddNotificationRuleDto
{
    public Guid ServiceId { get; set; }
    
    public NotificationChannel? Channel { get; set; }   
    
    public string Target { get; set; }      
    
    public bool NotifyOnFailure { get; set; } 
    
    public bool NotifyOnRecovery { get; set; }
}