using Microsoft.AspNetCore.Http;
using NewerDown.Domain.DTOs.File;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<FileAttachmentResponseDto> UploadFileAsync(IFormFile file);
    Task<FileAttachment> GetFileAttachmentByIdAsync(Guid? fileAttachmentId);
    Task DeleteFileAsync(Guid? fileAttachmentId);
}