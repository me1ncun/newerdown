using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Controllers;

[Authorize]
[ApiController]
[Route("api/users/me")]
public class UserController : ControllerBase
{
    private readonly ISignInService _signInService;
    private readonly IUserPhotoProvider _userPhotoProvider;
    private readonly IUserService _userService;
    
    public UserController(
        ISignInService signInService,
        IUserPhotoProvider userPhotoProvider,
        IUserService userService)
    {
        _signInService = signInService;
        _userPhotoProvider = userPhotoProvider;
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userService.GetCurrentUserAsync();
        
        return Ok(user);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        await _userService.DeleteUserAsync();
        
        return Ok();
    }
    
    [HttpPost("change-password")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        await _signInService.ChangePasswordAsync(request);

        return Ok("Password changed successfully.");
    }
    
    [HttpPost("upload-photo")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> UploadUserPhoto(IFormFile file)
    {
        await _userPhotoProvider.UploadPhotoAsync(file);
        
        return Ok("Photo uploaded successfully.");
    }
    
    [HttpDelete("delete-photo")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteUserPhoto()
    {
        await _userPhotoProvider.DeletePhotoAsync();
        
        return Ok("Photo deleted successfully.");
    }
}