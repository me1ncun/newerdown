using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.Enums;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = nameof(RoleType.Administrator))]
public class AdminController : ControllerBase
{
    private readonly IMonitorService _monitorService;
    private readonly IUserService _userService;
    
    public AdminController(
        IMonitorService monitorService,
        IUserService userService)
    {
        _monitorService = monitorService;
        _userService = userService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var response = await _userService.GetAllUsersAsync();
        return Ok(response);
    }

    [HttpGet("monitors")]
    public async Task<IActionResult> GetMonitors()
    {
        var response = await _monitorService.GetAllMonitorsAsync();
        return Ok(response);
    }
}