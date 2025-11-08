using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Interfaces;
using NewerDown.Extensions;

namespace NewerDown.Controllers;

/// <summary>
/// Provides API endpoints for managing the current authenticated user and their profile data.
/// </summary>
[Authorize]
[ApiController]
[Route("api/users/me")]
public class UserController : ControllerBase
{
    private readonly IUserPhotoProvider _userPhotoProvider;
    private readonly IUserService _userService;
    private readonly IUserContextService _userContextService;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userPhotoProvider">The service that handles user photo upload and deletion.</param>
    /// <param name="userService">The service responsible for user operations (update, delete, etc.).</param>
    /// <param name="userContextService">The service that retrieves information about the currently authenticated user.</param>
    public UserController(
        IUserPhotoProvider userPhotoProvider,
        IUserService userService,
        IUserContextService userContextService)
    {
        _userPhotoProvider = userPhotoProvider;
        _userService = userService;
        _userContextService = userContextService;
    }
    
    /// <summary>
    /// Retrieves details of the currently authenticated user.
    /// </summary>
    /// <returns>Returns the current user's information.</returns>
    /// <response code="200">Successfully retrieved user information.</response>
    /// <response code="400">Bad request or invalid authentication context.</response>
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await _userContextService.GetCurrentUserAsync();
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Deletes the currently authenticated user from the system.
    /// </summary>
    /// <returns>Returns status of the delete operation.</returns>
    /// <response code="204">User successfully deleted.</response>
    /// <response code="400">Bad request or failed deletion.</response>
    [HttpDelete]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteUser()
    {
        var result = await _userService.DeleteUserAsync();
        Response.Cookies.Delete("refreshToken");
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Updates profile information for the current user.
    /// </summary>
    /// <param name="request">The updated user information.</param>
    /// <returns>Returns the updated user data.</returns>
    /// <response code="200">User information successfully updated.</response>
    /// <response code="400">Invalid input data.</response>
    [HttpPatch]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto request)
    {
        var result = await _userService.UpdateUserAsync(request);
        return result.ToDefaultApiResponse();
    }
    
    /// <summary>
    /// Uploads a new profile photo for the current user.
    /// </summary>
    /// <param name="file">The photo file to upload.</param>
    /// <returns>Returns the URL or identifier of the uploaded photo.</returns>
    /// <response code="200">Photo successfully uploaded.</response>
    /// <response code="400">Invalid file or upload failed.</response>
    [HttpPost("upload-photo")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> UploadUserPhoto(IFormFile file)
    {
        var result = await _userPhotoProvider.UploadPhotoAsync(file);
        return Ok(result);
    }
    
    /// <summary>
    /// Deletes the current user's profile photo.
    /// </summary>
    /// <returns>Returns status of the photo deletion.</returns>
    /// <response code="204">Photo successfully deleted.</response>
    /// <response code="400">Failed to delete photo.</response>
    [HttpDelete("delete-photo")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(string))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteUserPhoto()
    {
        var result = await _userPhotoProvider.DeletePhotoAsync();
        return result.ToDefaultApiResponse();
    }
}