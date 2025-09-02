using System.ComponentModel.DataAnnotations.Schema;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.Entities;

public class Alert
{
    public Guid Id { get; set; }
    
    public AlertType Type { get; set; }
    
    public string Target { get; set; } = default!; // Email address or URL

    public Guid MonitorId { get; set; }
    
    public Monitor Monitor { get; set; } = default!;
    

    public DateTime? LastTriggeredAt { get; set; }

    public Guid UserId { get; set; }
    
    public User User { get; set; } = default!;
}