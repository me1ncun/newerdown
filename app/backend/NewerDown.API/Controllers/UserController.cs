using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Interfaces;
using NewerDown.Extensions;

namespace NewerDown.Controllers;

[Authorize]
[ApiController]
[Route("api/users/me")]
public class UserController : ControllerBase
{
    private readonly IUserPhotoProvider _userPhotoProvider;
    private readonly IUserService _userService;
    private readonly IUserContextService _userContextService;
    
    public UserController(
        IUserPhotoProvider userPhotoProvider,
        IUserService userService,
        IUserContextService userContextService)
    {
        _userPhotoProvider = userPhotoProvider;
        _userService = userService;
        _userContextService = userContextService;
    }
    
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userContextService.GetCurrentUserAsync();
        
        return Ok(user);
    }
    
    [HttpDelete]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteUser()
    {
        var result = await _userService.DeleteUserAsync();
        
        return result.ToDefaultApiResponse();
    }
    
    [HttpPatch]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto request)
    {
        var result = await _userService.UpdateUserAsync(request);
        
        return result.ToDefaultApiResponse();
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
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteUserPhoto()
    {
        var result = await _userPhotoProvider.DeletePhotoAsync();
        
        return result.ToDefaultApiResponse();
    }
}