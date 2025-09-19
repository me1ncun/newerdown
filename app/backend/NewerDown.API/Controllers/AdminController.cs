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
    public async Task<IActionResult> GetMonitors()
    {
        var monitors = await _adminService.GetAllMonitorsAsync();
        return Ok(monitors);
    }
}