using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.DTOs.Email;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Queuing;

namespace NewerDown.Controllers;

[ApiController]
[Route("/api")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ISignInService _signInService;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly IQueueSenderFactory _senderFactory;

    public AccountController(SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        UserManager<User> userManager,
        ISignInService signInService,
        IAuthService authService,
        IUserService userService,
        IQueueSenderFactory senderFactory)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _signInService = signInService;
        _authService = authService;
        _userService = userService;
        _senderFactory = senderFactory;
    }

    [AllowAnonymous]
    [HttpPost("account/login")]
    public async Task<IActionResult> Login([FromBody] LoginAccountDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return BadRequest("Invalid credentials.");
        }
        
        var token = _authService.GenerateToken(user);

        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("account/register")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            _logger.LogWarning("User {name} registration failed: {errors}", registerDto.UserName,
                string.Join(", ", result.Errors.Select(e => e.Description)));
            return BadRequest(result.Errors);
        }

        _logger.LogInformation("User {name} registered successfully.", registerDto.UserName);
        
        await _signInManager.SignInAsync(user, isPersistent: false);
        
        var sender = _senderFactory.Create(QueueType.Emails.GetQueueName());
        var email = new EmailDto(user.Email, user.UserName, DateTime.UtcNow);
        await sender.SendAsync(email, sessionId: email.Id);

        return Ok();
    }

    [Authorize]
    [HttpPost("account/logout")]
    public async Task<IActionResult> Logout()
    {
        var username = _userManager.GetUserName(User);
        await _signInManager.SignOutAsync();

        _logger.LogInformation("User {name} logged out.", username);

        return Ok();
    }

    [Authorize]
    [HttpPost("account/change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        await _signInService.ChangePassword(
            _userManager.GetUserId(User),
            changePasswordDto.OldPassword,
            changePasswordDto.NewPassword);

        return Ok("Password changed successfully.");
    }
    
    [Authorize]
    [HttpGet("account/current")]
    public IActionResult GetCurrentUserId()
    {
        var userId = _userService.GetUserId();
        
        return Ok(userId);
    }
}