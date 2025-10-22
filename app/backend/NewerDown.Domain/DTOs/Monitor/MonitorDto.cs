using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Service;

public class MonitorDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Url { get; set; }
    
    public int CheckIntervalSeconds { get; set; }
    
    public bool IsActive { get; set; }
    
    public MonitorType Type { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Guid UserId { get; set; }
}