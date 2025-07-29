using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ISignInService _signInService;
    
    public AuthenticationController(ISignInService signInService)
    {
        _signInService = signInService;
    }
    
    [HttpPost("register")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(void))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        await _signInService.RegisterUserAsync(registerDto);
        
        return Ok();
    }
    
    [HttpPost("login")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        var result = await _signInService.LoginUserAsync(loginDto);

        return Ok(result);
    }
}