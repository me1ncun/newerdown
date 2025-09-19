using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewerDown.Application.Time;
using NewerDown.Domain.DTOs.Incidents;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class IncidentService : IIncidentService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IScopedTimeProvider _timeProvider;
    
    public IncidentService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        IScopedTimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _timeProvider = timeProvider;
    }
    
    public async Task<List<IncidentDto>> GetAllAsync(Guid userId)
    {
        var incidents = await _dbContext.Incidents
            .Include(x => x.Monitor)
            .Where(x => x.Monitor.UserId == userId
                                         && x.StartedAt < _timeProvider.UtcNow())
            .ToListAsync();
        
        return _mapper.Map<List<IncidentDto>>(incidents);
    }

    public async Task<IncidentDto> GetByIdAsync(Guid id, Guid userId)
    {
        var incident = await _dbContext.Incidents
            .Include(x => x.Monitor)
            .FirstOrDefaultAsync(x => x.Id == id
                                      && x.Monitor.UserId == userId
                                      && x.StartedAt < _timeProvider.UtcNow());
        
        return _mapper.Map<IncidentDto>(incident);
    }

    public async Task AcknowledgeIncidentAsync(Guid id, Guid userId)
    {
        var incident = await _dbContext.Incidents
            .Include(x => x.Monitor)
            .FirstOrDefaultAsync(x => x.Id == id
                                      && x.Monitor.UserId == userId
                                      && x.StartedAt < _timeProvider.UtcNow());
        if (incident is null)
        {
            throw new EntityNotFoundException($"Incident not found with the id {id}.");
        }
        
        incident.IsAcknowledged = true;
        
        _dbContext.Incidents.Update(incident);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CommentIncidentAsync(CreateIncidentCommentDto comment)
    {
        var incidentComment = await _dbContext.IncidentComments
            .Include(x => x.Incident)
            .ThenInclude(x => x.Monitor)
            .FirstOrDefaultAsync(x => x.IncidentId == comment.IncidentId 
                                      && x.Incident.Monitor.UserId == comment.UserId);

        if (incidentComment is not null)
        {
            throw new EntityNotFoundException($"Incident comment has already been created with id {comment.IncidentId}.");
        }

        var entity = _mapper.Map<IncidentComment>(comment);
        entity.Id = Guid.NewGuid();
        
        _dbContext.IncidentComments.Add(entity);
        await _dbContext.SaveChangesAsync();
    }
}