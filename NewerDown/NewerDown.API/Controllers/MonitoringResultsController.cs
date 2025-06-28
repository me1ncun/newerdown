using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[Authorize]
[ApiController]
[Route("/api")]
public class MonitoringResultsController : ControllerBase
{
    private readonly IMonitoringResultService _service;
    
    public MonitoringResultsController(
        IMonitoringResultService service)
    {
        _service = service;
    }
    
    [HttpGet("results")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<MonitoringResultDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitoringResults(
        [FromQuery] string? filter = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
            var results = await _service.GetMonitoringResultsAsync(filter, page, pageSize);
            return Ok(results);
    }
    
    [HttpGet("results/stats/{days}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<MonitoringResultDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitoringResultsByDays(int days)
    {
        var results = await _service.GetMonitoringResultsByDaysAsync(days);
        return Ok(results);
    }
}