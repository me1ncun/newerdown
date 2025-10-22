using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Service;

public class AddMonitorDto
{
    public string Name { get; set; }
    
    public string Target { get; set; } = default!;
    
    public MonitorType Type { get; set; }
    
    public int? Port { get; set; }
    
    public int IntervalSeconds { get; set; }
    
    public bool IsActive { get; set; }
}