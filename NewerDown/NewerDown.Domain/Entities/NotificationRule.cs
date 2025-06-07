using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewerDown.Domain.Entities;

public class NotificationRule
{
    public Guid Id { get; set; }
    
    public Guid ServiceId { get; set; }
    
    public Service Service { get; set; }
    
    public NotificationChannel Channel { get; set; }   
    
    public string Target { get; set; }      
    
    public bool NotifyOnFailure { get; set; } 
    
    public bool NotifyOnRecovery { get; set; }
    
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}

public enum NotificationChannel
{
    [Description("Email")] 
    Email,
    
    [Description("SMS")] 
    SMS,
    
    [Description("Push Notification")] 
    PushNotification,
    
    [Description("Telegram")] 
    Telegram
}