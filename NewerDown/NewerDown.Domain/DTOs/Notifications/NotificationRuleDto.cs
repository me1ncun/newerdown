using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Notifications;

public class NotificationRuleDto
{
    public Guid Id { get; set; }
    
    public Guid ServiceId { get; set; }
    
    public NotificationChannel Channel { get; set; }   
    
    public string Target { get; set; }      
    
    public bool NotifyOnFailure { get; set; } 
    
    public bool NotifyOnRecovery { get; set; }
    
    public Guid UserId { get; set; }
}