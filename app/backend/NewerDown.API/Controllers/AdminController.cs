using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
    [Authorize(Roles = nameof(RoleType.Administrator))]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _adminService.GetAllUsersAsync();
        
        return Ok(users);
    }
    
    [Authorize(Roles = nameof(RoleType.Administrator))]
    [HttpGet("monitors")]
    public IActionResult GetMonitors()
    {
        // Logic to get all monitors
        return Ok("List of monitors retrieved successfully");
    }
    
    [Authorize(Roles = nameof(RoleType.Administrator))]
    [HttpGet("logs")]
    public IActionResult GetSystemLogs()
    {
        // Logic to get all logs
        return Ok("List of logs retrieved successfully");
    }
}