using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.MonitorCheck;

public class MonitorTypeDto
{
    public Guid Id { get; set; }
    public MonitorType Type { get; set; }
}