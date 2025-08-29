using NewerDown.Domain.DTOs.Incidents;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application.Services;

public class IncidentService : IIncidentService
{
    public Task<IEnumerable<IncidentDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IncidentDto> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task AcknowledgeIncidentAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task CommentIncidentAsync(Guid id, IncidentCommentDto comment)
    {
        throw new NotImplementedException();
    }
}