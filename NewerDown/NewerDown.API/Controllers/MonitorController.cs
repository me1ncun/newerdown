using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Interfaces;
using NewerDown.Shared.Validations;

namespace NewerDown.Controllers;

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
    public async Task<IActionResult> CreateMonitor([FromBody] AddMonitorDto monitorDto)
    {
        var validationResult = await _fluentValidator.ValidateAsync(monitorDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var result = await _monitorService.CreateMonitorAsync(monitorDto);
        
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllMonitors()
    {
        var monitors = await _monitorService.GetAllMonitors();
      
        return Ok(monitors);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMonitor(Guid id)
    {
        var monitor = await _monitorService.GetMonitorByIdAsync(id);
       
        return Ok(monitor);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMonitor(Guid id, [FromBody] UpdateMonitorDto monitorDto)
    {
        var validationResult = await _fluentValidator.ValidateAsync(monitorDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        await _monitorService.UpdateMonitorAsync(id, monitorDto);
        
        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMonitor(Guid id)
    {
        await _monitorService.DeleteMonitorAsync(id);
       
        return Ok();
    }
    
    [HttpPost("{id:guid}/pause")]
    public IActionResult PauseMonitor(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpPost("{id:guid}/resume")]
    public IActionResult ResumeMonitor(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpPost("import")]
    public IActionResult ImportMonitors(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/export")]
    public IActionResult ExportMonitor(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/history")]
    public IActionResult GetHistoryByMonitor(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/status")]
    public IActionResult GetMonitorStatus(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/summary")]
    public IActionResult GetMonitorSummary(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/downtimes")]
    public IActionResult GetMonitorDowntimes(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/uptime-percentage")]
    public IActionResult GetMonitorUptimePercentage(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
    
    [HttpGet("{id}/latency-path")]
    public IActionResult GetMonitorLatencyPath(Guid id)
    {
        // Logic to create a monitor
        return Ok("Monitor created successfully");
    }
}