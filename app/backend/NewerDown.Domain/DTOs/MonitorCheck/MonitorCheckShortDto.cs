namespace NewerDown.Domain.DTOs.MonitorCheck;

public class MonitorCheckShortDto
{
    public DateTime CheckedAt { get; set; }
    
    public double? ResponseTimeMs { get; set; }
}