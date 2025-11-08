using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.DTOs.Token;
using NewerDown.Domain.Interfaces;
using NewerDown.Extensions;

namespace NewerDown.Controllers;

/// <summary>
/// Provides endpoints for user authentication and authorization operations,
/// including login, registration, password management, and token refresh.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISignInService _signInService;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="signInService">The authentication service handling sign-in, sign-up, and token operations.</param>
    public AuthController(ISignInService signInService)
    {
        _signInService = signInService;
    }
    
    /// <summary>
    /// Refreshes the JWT access token using a valid refresh token.
    /// </summary>
    /// <returns>Returns a new access token if the refresh is successful.</returns>
    /// <response code="200">Token successfully refreshed.</response>
    /// <response code="401">Unauthorized — missing or invalid refresh token.</response>
    /// <response code="400">Invalid request or refresh operation failed.</response>
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
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(5)
        });

        return Ok(new { accessToken = result.AccessToken });
    }
    
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="registerUser">The registration details of the new user.</param>
    /// <returns>No content if registration succeeds.</returns>
    /// <response code="204">User successfully registered.</response>
    /// <response code="400">Validation failed or registration error occurred.</response>
    [HttpPost("signup")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> SignUp([FromBody] RegisterUserDto registerUser)
    {
        var result = await _signInService.SignUpUserAsync(registerUser);
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Authenticates a user using provided credentials and issues JWT and refresh tokens.
    /// </summary>
    /// <param name="loginUser">The user's login credentials.</param>
    /// <returns>Returns the access token on successful authentication.</returns>
    /// <response code="200">Login successful — returns the access token.</response>
    /// <response code="400">Invalid credentials or authentication failed.</response>
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
            Expires = DateTime.UtcNow.AddDays(5)
        });
        
        return Ok(result.Value.AccessToken);
    }
    
    /// <summary>
    /// Changes the password for the currently authenticated user.
    /// </summary>
    /// <param name="request">The request containing old and new password data.</param>
    /// <returns>No content if password change is successful.</returns>
    /// <response code="204">Password successfully changed.</response>
    /// <response code="400">Invalid input or password change failed.</response>
    [HttpPost("change-password")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var result = await _signInService.ChangePasswordAsync(request);
        return result.ToDefaultApiResponse();
    }
}