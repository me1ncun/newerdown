using System.ComponentModel;

namespace NewerDown.Domain.Enums;

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