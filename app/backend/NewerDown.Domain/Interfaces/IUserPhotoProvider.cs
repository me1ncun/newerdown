using Microsoft.AspNetCore.Http;
using NewerDown.Domain.Result;

namespace NewerDown.Domain.Interfaces;

public interface IUserPhotoProvider
{
    Task UploadPhotoAsync(IFormFile file);
    Task<Result<string>> GetPhotoUrlAsync();
    Task<Result.Result> DeletePhotoAsync();
}