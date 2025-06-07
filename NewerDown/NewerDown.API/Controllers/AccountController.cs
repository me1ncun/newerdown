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
    private readonly ILogger _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ISignInService _signInService;

    public AccountController(SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        UserManager<User> userManager,
        ISignInService signInService)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _signInService = signInService;
    }

    [AllowAnonymous]
    [HttpPost("account/login")]
    public async Task<IActionResult> Login([FromBody] LoginAccountDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user is null)
        {
            return BadRequest("User not found");
        }

        var result = await _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password,
            isPersistent: false, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            _logger.LogWarning("User {name} failed to log in.", loginDto.UserName);
            return BadRequest("Invalid login attempt.");
        }

        return Ok();
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

        return Ok();
    }

    [HttpPost("account/logout")]
    public async Task<IActionResult> Logout()
    {
        var username = _userManager.GetUserName(User);
        await _signInManager.SignOutAsync();

        _logger.LogInformation("User {name} logged out.", username);

        return Ok();
    }

    [HttpPost("account/change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        await _signInService.ChangePassword(
            _userManager.GetUserId(User),
            changePasswordDto.OldPassword,
            changePasswordDto.NewPassword);

        return Ok("Password changed successfully.");
    }
}