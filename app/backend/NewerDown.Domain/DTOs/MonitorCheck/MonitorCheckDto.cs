using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.MonitorCheck;

public class MonitorCheckDto
{
    public Guid Id { get; set; }
    
    public Guid MonitorId { get; set; }
    
    public DateTime CheckedAt { get; set; }
    
    public MonitorStatus Status { get; set; }
    
    public string? StatusCode { get; set; }
    
    public bool IsSuccess { get; set; }
    
    public double? ResponseTimeMs { get; set; }
    
    public string? ErrorMessage { get; set; }
}