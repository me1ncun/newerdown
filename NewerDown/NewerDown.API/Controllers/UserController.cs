using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[ApiController]
[Route("api/users/me")]
public class UserController : ControllerBase
{
    private readonly ISignInService _signInService;
    private readonly IUserPhotoProvider _userPhotoProvider;
    
    public UserController(ISignInService signInService, IUserPhotoProvider userPhotoProvider)
    {
        _signInService = signInService;
        _userPhotoProvider = userPhotoProvider;
    }
    
    [HttpGet]
    public IActionResult GetCurrentUser([FromBody] object registrationData)
    {
        return Ok("Registration successful");
    }
    
    [HttpPut("{id:guid}")]
    public IActionResult UpdateUser(Guid id, [FromBody] object loginData)
    {
        return Ok("Login successful");
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteUser(Guid id, [FromBody] object loginData)
    {
        return Ok("Login successful");
    }
    
    [HttpPost("account/change-password")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        await _signInService.ChangePasswordAsync(request);

        return Ok("Password changed successfully.");
    }
    
    [HttpPost("account/upload-photo")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> UploadPhoto(IFormFile file)
    {
        await _userPhotoProvider.UploadPhotoAsync(file);
        
        return Ok("Photo uploaded successfully.");
    }
    
    [HttpDelete("account/delete-photo")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeletePhoto()
    {
        await _userPhotoProvider.DeletePhotoAsync();
        
        return Ok("Photo deleted successfully.");
    }
}