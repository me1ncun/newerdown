namespace NewerDown.Domain.DTOs.Incidents;

public class CreateIncidentCommentDto
{
    public Guid IncidentId { get; set; }

    public Guid UserId { get; set; }
    
    public string Comment { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
}