using NewerDown.Domain.Enums;

namespace NewerDown.Domain.Entities;

public class MonitorCheck
{
    public Guid Id { get; set; }
    
    public Guid MonitorId { get; set; }
    
    public Monitor Monitor { get; set; }
    
    public DateTime CheckedAt { get; set; }
    
    public MonitorStatus Status { get; set; }
    
    public double? ResponseTimeMs { get; set; }
    
    public string? ErrorMessage { get; set; }
}