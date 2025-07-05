using Microsoft.AspNetCore.Http;
using NewerDown.Domain.DTOs.File;

namespace NewerDown.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<FileAttachmentResponseDto> UploadFileAsync(IFormFile file);
}