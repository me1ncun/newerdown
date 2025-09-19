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
    
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<MonitorDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetAllMonitors()
    {
        var result = await _monitorService.GetAllMonitorsAsync();
      
        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorById(GetByIdDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _monitorService.GetMonitorByIdAsync(request);
        
        return result.ToDefaultApiResponse();
    }
    
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
    
    [HttpPost("resume")]
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
    
    [HttpPost("import")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ImportMonitorsCsv(IFormFile file)
    {
        await _monitorService.ImportMonitorsFromCsvAsync(file);
        return Ok();
    }
    
    [HttpGet("export")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ExportMonitorCsv(GetByIdDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var fileContent = await _monitorService.ExportMonitorCsvAsync(request);
        
        return File(fileContent, "text/csv", $"monitor_{request.Id}.csv");
    }
    
    [HttpGet("{id}/history")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(PagedList<MonitorCheckDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetHistoryByMonitor(Guid id, int pageNumber, int pageSize)
    {
        var history = await _monitorService.GetHistoryByMonitorAsync(id, pageNumber, pageSize);
        return Ok(history);
    }
    
    [HttpGet("status")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorStatus))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorStatus(GetByIdDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var status = await _monitorService.GetMonitorStatusAsync(request);
        
        return Ok(status);
    }
    
    [HttpGet("{id}/summary")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MonitorSummaryDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorSummary(Guid id, int hours)
    {
        var summary = await _monitorService.GetMonitorSummaryAsync(id, hours);
        return Ok(summary);
    }
    
    [HttpGet("downtimes")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<DownTimeDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorDowntimes(GetByIdDto request)
    {
        var validationResult = await _fluentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var downTimes = await _monitorService.GetDownTimesAsync(request);
        
        return Ok(downTimes);
    }
    
    [HttpGet("{id}/uptime-percentage")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UptimePercentageDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorUptimePercentage(Guid id, DateTime from, DateTime to)
    {
        var result = await _monitorService.GetUptimePercentageAsync(id, from, to);
        return Ok(result);
    }
    
    [HttpGet("{id}/latency-path")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(List<MonitorCheckShortDto>))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetMonitorLatencyGraph(Guid id, DateTime from, DateTime to)
    {
        var results = await _monitorService.GetLatencyGraph(id, from, to);
        return Ok(results);
    }
}