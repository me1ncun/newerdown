using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.MonitorCheck;
using NewerDown.Domain.DTOs.MonitoringResults;
using NewerDown.Domain.DTOs.Request;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Interfaces;
using NewerDown.Domain.Paging;
using NewerDown.Extensions;
using NewerDown.Shared.Validations;

namespace NewerDown.Controllers;

/// <summary>
/// Provides API endpoints for managing monitors, including creation, updates,
/// monitoring status retrieval, CSV import/export, and performance statistics.
/// </summary>
[Authorize]
[ApiController]
[Route("api/monitors")]
public class MonitorController : ControllerBase
{
    private readonly IMonitorService _monitorService;
    private readonly IFluentValidator _fluentValidator;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MonitorController"/> class.
    /// </summary>
    /// <param name="monitorService">The service handling monitor operations.</param>
    /// <param name="fluentValidator">The service used for validating incoming DTOs.</param>
    public MonitorController(IMonitorService monitorService, IFluentValidator fluentValidator)
    {
        _monitorService = monitorService;
        _fluentValidator = fluentValidator;
    }
    
    /// <summary>
    /// Creates a new monitor.
    /// </summary>
    /// <param name="request">The monitor creation request payload.</param>
    /// <returns>The ID of the created monitor.</returns>
    /// <response code="200">Monitor successfully created.</response>
    /// <response code="400">Validation failed or invalid request data.</response>
    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> CreateMonitor([FromBody] AddMonitorDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _monitorService.CreateMonitorAsync(request);
        
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Retrieves all monitors.
    /// </summary>
    /// <returns>List of monitors.</returns>
    /// <response code="200">Successfully retrieved all monitors.</response>
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<MonitorDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetAllMonitors()
    {
        var result = await _monitorService.GetAllMonitorsAsync();
      
        return Ok(result);
    }
    
    /// <summary>
    /// Retrieves a specific monitor by its ID.
    /// </summary>
    /// <param name="id">The monitor ID.</param>
    /// <returns>The monitor details.</returns>
    /// <response code="200">Monitor found.</response>
    /// <response code="400">Invalid or missing ID.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorById(Guid id)
    {
        if(id == Guid.Empty)
            return BadRequest("Id cannot be empty");
        
        var result = await _monitorService.GetMonitorByIdAsync(id);
        
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Updates an existing monitor.
    /// </summary>
    /// <param name="id">The monitor ID.</param>
    /// <param name="request">The updated monitor data.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Monitor successfully updated.</response>
    /// <response code="400">Invalid input or failed validation.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateMonitor(Guid id, [FromBody] UpdateMonitorDto request)
    {
        if(id == Guid.Empty)
            return BadRequest("Id cannot be empty");
        
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _monitorService.UpdateMonitorAsync(id, request);
        
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Deletes an existing monitor.
    /// </summary>
    /// <param name="request">The monitor deletion request data.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Monitor successfully deleted.</response>
    /// <response code="400">Invalid request data.</response>
    [HttpDelete]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteMonitor(DeleteMonitorDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _monitorService.DeleteMonitorAsync(request);
       
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Pauses a monitor.
    /// </summary>
    /// <param name="request">The monitor ID wrapper.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Monitor successfully paused.</response>
    /// <response code="400">Invalid request data.</response>
    [HttpPost("pause")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> PauseMonitor([FromBody] GetByIdDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _monitorService.PauseMonitorAsync(request);
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Resumes a paused monitor.
    /// </summary>
    /// <param name="request">The monitor ID wrapper.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Monitor successfully resumed.</response>
    /// <response code="400">Invalid request data.</response>
    [HttpPost("{id}/resume")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ResumeMonitor(GetByIdDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _monitorService.ResumeMonitorAsync(request);
        
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Imports monitors from a CSV file.
    /// </summary>
    /// <param name="file">The uploaded CSV file.</param>
    /// <returns>Status of the import operation.</returns>
    /// <response code="200">CSV file successfully imported.</response>
    /// <response code="400">Invalid file format or parsing error.</response>
    [HttpPost("import")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ImportMonitorsCsv(IFormFile file)
    {
        await _monitorService.ImportMonitorsFromCsvAsync(file);
        return Ok();
    }
    
    /// <summary>
    /// Exports monitor data to CSV.
    /// </summary>
    /// <param name="id">The monitor ID.</param>
    /// <returns>The CSV file as a downloadable stream.</returns>
    /// <response code="200">CSV file successfully generated.</response>
    /// <response code="400">Invalid or missing monitor ID.</response>
    [HttpPost("{id}/export")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ExportMonitorCsv(Guid id)
    {
        if(id == Guid.Empty)
            return BadRequest("Id cannot be empty");
        
        var fileContent = await _monitorService.ExportMonitorCsvAsync(id);
        
        return File(fileContent, "text/csv", $"monitor_{id}.csv");
    }
    
    /// <summary>
    /// Retrieves the monitoring history for a specific monitor.
    /// </summary>
    /// <param name="id">The monitor ID.</param>
    /// <param name="pageNumber">Page number for pagination.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <returns>Paged list of monitor check history.</returns>
    [HttpGet("{id}/history")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(PagedList<MonitorCheckDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetHistoryByMonitor(Guid id, int pageNumber, int pageSize)
    {
        var history = await _monitorService.GetHistoryByMonitorAsync(id, pageNumber, pageSize);
        return Ok(history);
    }
    
    /// <summary>
    /// Retrieves the current status of a monitor.
    /// </summary>
    /// <param name="id">The monitor ID.</param>
    /// <returns>The monitor status (Active, Paused, etc.).</returns>
    [HttpGet("{id}/status")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorStatus))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorStatus(Guid id)
    {
        if(id == Guid.Empty)
            return BadRequest("Id cannot be empty");
        
        var status = await _monitorService.GetMonitorStatusAsync(id);
        
        return Ok(status);
    }
    
    /// <summary>
    /// Retrieves a summary of monitoring statistics over a given period.
    /// </summary>
    /// <param name="id">The monitor ID.</param>
    /// <param name="hours">The number of hours to include in the summary.</param>
    /// <returns>The monitor summary.</returns>
    [HttpGet("{id}/summary")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorSummaryDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorSummary(Guid id, int hours)
    {
        var summary = await _monitorService.GetMonitorSummaryAsync(id, hours);
        return Ok(summary);
    }
    
    /// <summary>
    /// Retrieves downtime records for a monitor.
    /// </summary>
    /// <param name="id">The monitor ID.</param>
    /// <returns>List of downtime entries.</returns>
    [HttpGet("{id}/downtimes")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DownTimeDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorDowntimes(Guid id)
    {
        if(id == Guid.Empty)
            return BadRequest("Id cannot be empty");
        
        var downTimes = await _monitorService.GetDownTimesAsync(id);
        
        return Ok(downTimes);
    }
    

    /// <summary>
    /// Retrieves the uptime percentage for a monitor within a given date range.
    /// </summary>
    /// <param name="id">The monitor ID.</param>
    /// <param name="from">Start date of the interval.</param>
    /// <param name="to">End date of the interval.</param>
    /// <returns>The uptime percentage.</returns>
    [HttpGet("{id}/uptime-percentage")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UptimePercentageDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorUptimePercentage(Guid id, DateTime from, DateTime to)
    {
        var result = await _monitorService.GetUptimePercentageAsync(id, from, to);
        return Ok(result);
    }
    
    /// <summary>
    /// Retrieves latency graph data for a monitor over a specified time period.
    /// </summary>
    /// <param name="id">The monitor ID.</param>
    /// <param name="from">Start date of the interval.</param>
    /// <param name="to">End date of the interval.</param>
    /// <returns>List of monitor latency records.</returns>
    [HttpGet("{id}/latency-path")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<MonitorCheckShortDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorLatencyGraph(Guid id, DateTime from, DateTime to)
    {
        var results = await _monitorService.GetLatencyGraph(id, from, to);
        return Ok(results);
    }
}