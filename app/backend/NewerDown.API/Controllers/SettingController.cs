using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Controllers;

[ApiController]
[Route("api")]
public class SettingController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SettingController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("check-types")]
    public async Task<IActionResult> CheckTypes()
    {
        var checkTypes = await _context.Monitors.Select(m => new
        { 
            m.Id,
            CheckType = m.Type
        }).ToListAsync();
        
        return Ok(checkTypes);
    }
    
    [HttpGet("check-intervals")]
    public async Task<IActionResult> CheckIntervals()
    {
        var checkIntervals = await _context.Monitors.Select(m => new
        { 
            m.Id,
            CheckInterval = m.IntervalSeconds
        }).ToListAsync();
        
        return Ok(checkIntervals);
    }
    
    [HttpGet("status-codes")]
    public async Task<IActionResult> StatusCodes()
    {
        var statusCodes = await _context.MonitorChecks.Select(mc => mc.StatusCode).Distinct().ToListAsync();
        return Ok(statusCodes);
    }
}