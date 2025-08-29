namespace NewerDown.Domain.DTOs.MonitoringResults;

public class MonitoringResultDto
{
    public Guid ServiceId { get; set; }
    public DateTime CheckedAt { get; set; }
    public int StatusCode { get; set; }
    public double ResponseTimeMs { get; set; } 
    public bool IsAlive { get; set; } 
    public string Error { get; set; } 
}