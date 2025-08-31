using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NewerDown.Application.Errors;
using NewerDown.Application.Extensions;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;
using NewerDown.Domain.Result;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Application.Services;

public class UserPhotoProvider : IUserPhotoProvider
{
    private readonly IUserService _userService;
    private readonly IUserContextService _userContextService;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger<UserPhotoProvider> _logger;
    private readonly ApplicationDbContext _context;
    
    public UserPhotoProvider(
        IUserService userService,
        IUserContextService userContextService,
        IBlobStorageService blobStorageService,
        ILogger<UserPhotoProvider> logger,
        ApplicationDbContext context)
    {
        _userService = userService;
        _userContextService = userContextService;
        _blobStorageService = blobStorageService;
        _logger = logger;
        _context = context;
    }

    public async Task UploadPhotoAsync(IFormFile file)
    {
        var userId = _userContextService.GetUserId();
        var user = (await _userService.GetUserByIdAsync(userId)).ThrowIfNull();
        
        var uploadedPhoto = await _blobStorageService.UploadFileAsync(file);
        
        user.FileAttachmentId = uploadedPhoto.FileAttachment.Id;
        
        _logger.LogInformation("User {UserId} uploaded a new photo: {PhotoUrl}", userId, uploadedPhoto.FileAttachment.Uri);
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Result<string>> GetPhotoUrlAsync()
    {
        var userId = _userContextService.GetUserId();
        var user = (await _userService.GetUserByIdAsync(userId)).ThrowIfNull();
        if (user.FileAttachmentId == Guid.Empty)
        {
            return Result<string>.Failure(PhotoErrors.UserPhotoNotFound);
        }

        var fileAttachment = await _blobStorageService.GetFileAttachmentByIdAsync(user.FileAttachmentId);
        
        return Result<string>.Success(fileAttachment.Uri);
    }
    
    public async Task<Result> DeletePhotoAsync()
    {
        var userId = _userContextService.GetUserId();
        var user = (await _userService.GetUserByIdAsync(userId)).ThrowIfNull();
        
        if (user.FileAttachmentId == Guid.Empty)
        {
            return Result.Failure(PhotoErrors.UserPhotoNotFound);
        }

        await _blobStorageService.DeleteFileAsync(user.FileAttachmentId);
        
        _logger.LogInformation("User {UserId} deleted their photo.", userId);
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }
}