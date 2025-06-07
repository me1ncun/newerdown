using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[Authorize]
[ApiController]
[Route("/api")]
public class MonitoringResultsController : ControllerBase
{
    private readonly ILogger<MonitoringResultsController> logger;
    private readonly IMonitoringResultService _service;
    
    public MonitoringResultsController(
        IMonitoringResultService service,
        ILogger<MonitoringResultsController> logger)
    {
        _service = service;
        this.logger = logger;
    }
    
    [HttpGet("results")]
    public async Task<IActionResult> GetMonitoringResults(
        [FromQuery] string? filter = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
            var results = await _service.GetMonitoringResultsAsync(filter, page, pageSize);
            return Ok(results);
    }

    [HttpGet("results/stats/{days}")]
    public async Task<IActionResult> GetMonitoringResultsByDays(int days)
    {
        var results = await _service.GetMonitoringResultsByDaysAsync(days);
        return Ok();
    }
}