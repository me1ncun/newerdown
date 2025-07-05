using System.ComponentModel.DataAnnotations.Schema;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.Entities;

public class NotificationRule
{
    public Guid Id { get; set; }
    
    public Guid ServiceId { get; set; }
    
    public NotificationChannel Channel { get; set; }   
    
    public string? Target { get; set; }   
    
    public bool NotifyOnFailure { get; set; } 
    
    public bool NotifyOnRecovery { get; set; }
    
    public bool IsActive { get; set; }
    
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
    public Service Service { get; set; }
}