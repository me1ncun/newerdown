namespace NewerDown.Domain.DTOs.Incidents;

public class IncidentDto
{
    public Guid Id { get; set; }

    public Guid MonitorId { get; set; }
    
    public DateTime StartedAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }

    public string? RootCause { get; set; }
    
    public string? ResolutionComment { get; set; }
    
    public bool IsAcknowledged { get; set; }
}