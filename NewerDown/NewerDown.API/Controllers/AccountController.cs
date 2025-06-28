using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[Authorize]
[ApiController]
[Route("/api")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly ISignInService _signInService;
    private readonly IUserService _userService;

    public AccountController(
        SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        ISignInService signInService,
        IUserService userService)
    {
        _logger = logger;
        _signInManager = signInManager;
        _signInService = signInService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("account/login")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> Login([FromBody] LoginAccountDto request)
    {
        var result = await _signInService.LoginUserAsync(request);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("account/register")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> Register([FromBody] RegisterAccountDto registerDto)
    {
        await _signInService.RegisterUserAsync(registerDto);
        
        return Ok();
    }
    
    [HttpPost("account/logout")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        
        var currentUserId = _userService.GetUserId();

        _logger.LogInformation("User with {id} logged out.", currentUserId);

        return Ok();
    }
    
    [HttpPost("account/change-password")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        await _signInService.ChangePasswordAsync(request);

        return Ok("Password changed successfully.");
    }
    
    [HttpGet("account/current")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(Guid))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public IActionResult GetCurrentUserId()
    {
        var userId = _userService.GetUserId();
        
        return Ok(userId);
    }
}