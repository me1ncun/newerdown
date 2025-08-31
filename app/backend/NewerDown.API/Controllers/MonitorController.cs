using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Interfaces;
using NewerDown.Extensions;
using NewerDown.Shared.Validations;

namespace NewerDown.Controllers;

[Authorize]
[ApiController]
[Route("api/monitors")]
public class MonitorController : ControllerBase
{
    private readonly IMonitorService _monitorService;
    private readonly IFluentValidator _fluentValidator;
    
    public MonitorController(IMonitorService monitorService, IFluentValidator fluentValidator)
    {
        _monitorService = monitorService;
        _fluentValidator = fluentValidator;
    }
    
    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> CreateMonitor([FromBody] AddMonitorDto monitor)
    {
        var validationResult = await _fluentValidator.ValidateAsync(monitor);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _monitorService.CreateMonitorAsync(monitor);
        return result.ToDefaultApiResponse();
    }
    
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<MonitorDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetAllMonitors()
    {
        var result = await _monitorService.GetAllMonitors();
      
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorById(Guid id)
    {
        var result = await _monitorService.GetMonitorByIdAsync(id);
        return result.ToDefaultApiResponse();
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateMonitor(Guid id, [FromBody] UpdateMonitorDto monitor)
    {
        var validationResult = await _fluentValidator.ValidateAsync(monitor);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _monitorService.UpdateMonitorAsync(id, monitor);
        return result.ToDefaultApiResponse();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteMonitor(Guid id)
    {
        var result = await _monitorService.DeleteMonitorAsync(id);
       
        return result.ToDefaultApiResponse();
    }
    
    [HttpPost("{id:guid}/pause")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> PauseMonitor(Guid id)
    {
        var result = await _monitorService.PauseMonitorAsync(id);
        return result.ToDefaultApiResponse();
    }
    
    [HttpPost("{id:guid}/resume")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ResumeMonitor(Guid id)
    {
        var result = await _monitorService.ResumeMonitorAsync(id);
        return result.ToDefaultApiResponse();
    }
    
    [HttpPost("import")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ImportMonitorsCsv(IFormFile file)
    {
        await _monitorService.ImportMonitorsFromCsvAsync(file);
        return Ok();
    }
    
    [HttpGet("{id}/export")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ExportMonitorCsv(Guid id)
    {
        var fileContent = await _monitorService.ExportMonitorCsvAsync(id);
        return File(fileContent, "text/csv", $"monitor_{id}.csv");
    }
    
    [HttpGet("{id}/history")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public IActionResult GetHistoryByMonitor(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/status")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public IActionResult GetMonitorStatus(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/summary")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public IActionResult GetMonitorSummary(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/downtimes")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public IActionResult GetMonitorDowntimes(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/uptime-percentage")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public IActionResult GetMonitorUptimePercentage(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/latency-path")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public IActionResult GetMonitorLatencyPath(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
}