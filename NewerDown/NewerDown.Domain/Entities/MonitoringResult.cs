namespace NewerDown.Domain.Entities;

public class MonitoringResult
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
    public DateTime CheckedAt { get; set; }
    public int StatusCode { get; set; }
    public double ResponseTimeMs { get; set; } 
    public bool IsAlive { get; set; } 
    public string Error { get; set; } 
}