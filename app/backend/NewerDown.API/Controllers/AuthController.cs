using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.DTOs.Token;
using NewerDown.Domain.Interfaces;
using NewerDown.Extensions;

namespace NewerDown.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISignInService _signInService;
    
    public AuthController(ISignInService signInService)
    {
        _signInService = signInService;
    }
    
    [HttpPost("token/refresh")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]                  
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized();

        var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var tokenDto = new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };

        var result = await _signInService.RefreshTokenAsync(tokenDto);

        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(new { accessToken = result.AccessToken });
    }
    
    [HttpPost("signup")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> SignUp([FromBody] RegisterUserDto registerUser)
    {
        var result = await _signInService.SignUpUserAsync(registerUser);
        return result.ToDefaultApiResponse();
    }
    
    [HttpPost("login")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
    {
        var result = await _signInService.LoginUserAsync(loginUser);
        if(result.IsFailure)
            return BadRequest(result.Error);
        
        Response.Cookies.Append("refreshToken", result.Value.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        });
        
        return Ok(result.Value.AccessToken);
    }
    
    [HttpPost("change-password")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var result = await _signInService.ChangePasswordAsync(request);
        return result.ToDefaultApiResponse();
    }
}