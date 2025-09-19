using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Service;

public class UpdateMonitorDto
{
    public MonitorType Type { get; set; }
    
    public string Name { get; set; }

    public string Url { get; set; }

    public bool IsActive { get; set; }
}