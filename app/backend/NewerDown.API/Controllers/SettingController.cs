using Microsoft.AspNetCore.Mvc;

namespace NewerDown.Controllers;

[ApiController]
[Route("api")]
public class SettingController : ControllerBase
{
    [HttpGet("check-types")]
    public IActionResult CheckTypes()
    {
        // Logic to check types
        return Ok("Types checked successfully");
    }
    
    [HttpGet("check-intervals")]
    public IActionResult CheckIntervals()
    {
        // Logic to check types
        return Ok("Types checked successfully");
    }
    
    [HttpGet("status-codes")]
    public IActionResult StatusCodes()
    {
        // Logic to check types
        return Ok("Types checked successfully");
    }
    
    [HttpGet("monitor-types")]
    public IActionResult MonitorTypes()
    {
        // Logic to check types
        return Ok("Types checked successfully");
    }
}