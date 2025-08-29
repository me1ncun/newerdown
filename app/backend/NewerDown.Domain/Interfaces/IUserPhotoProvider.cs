using Microsoft.AspNetCore.Http;

namespace NewerDown.Domain.Interfaces;

public interface IUserPhotoProvider
{
    Task UploadPhotoAsync(IFormFile file);
    Task<string> GetPhotoUrlAsync();
    Task DeletePhotoAsync();
}