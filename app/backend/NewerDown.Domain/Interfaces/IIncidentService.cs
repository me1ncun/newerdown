using NewerDown.Domain.DTOs.Incidents;

namespace NewerDown.Domain.Interfaces;

public interface IIncidentService
{
    Task<IEnumerable<IncidentDto>> GetAllAsync();
    Task<IncidentDto> GetByIdAsync(Guid id);
    Task AcknowledgeIncidentAsync(Guid id);
    Task CommentIncidentAsync(Guid id, IncidentCommentDto comment);
}