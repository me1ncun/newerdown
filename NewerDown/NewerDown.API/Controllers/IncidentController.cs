using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Incidents;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[ApiController]
[Route("api/incidents")]
public class IncidentController : ControllerBase
{
    private readonly IIncidentService _incidentService;
    
    public IncidentController(IIncidentService incidentService)
    {
        _incidentService = incidentService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var incidents = await _incidentService.GetAllAsync();
        
        return Ok(incidents);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var incident = await _incidentService.GetByIdAsync(id);
        
        return Ok(incident);
    }
    
    [HttpPost("acknowledge/{id:guid}")]
    public async Task<IActionResult> AcknowledgeIncident(Guid id)
    {
        await _incidentService.AcknowledgeIncidentAsync(id);
        return Ok();
    }
    
    [HttpPost("comment/{id:guid}")]
    public async Task<IActionResult> CommentIncident(Guid id, [FromBody] IncidentCommentDto comment)
    {
        await _incidentService.CommentIncidentAsync(id, comment);
        return Ok();
    }
}