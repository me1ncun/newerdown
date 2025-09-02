namespace NewerDown.Domain.Entities;

public class Incident
{
    public Guid Id { get; set; }

    public Guid MonitorId { get; set; }
    
    public Monitor Monitor { get; set; } = default!;
    
    public DateTime StartedAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }

    public string? RootCause { get; set; }
    
    public string? ResolutionComment { get; set; }
    
    public bool IsAcknowledged { get; set; }

    public ICollection<IncidentComment> Comments { get; set; } = new List<IncidentComment>();
}