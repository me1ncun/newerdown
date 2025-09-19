using NewerDown.Domain.DTOs.Incidents;

namespace NewerDown.Domain.Interfaces;

public interface IIncidentService
{
    Task<List<IncidentDto>> GetAllAsync(Guid userId);
    Task<IncidentDto> GetByIdAsync(Guid id, Guid userId);
    Task AcknowledgeIncidentAsync(Guid id, Guid userId);
    Task CommentIncidentAsync(CreateIncidentCommentDto comment);
}