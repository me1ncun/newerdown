using Microsoft.AspNetCore.Http;
using NewerDown.Domain.Result;

namespace NewerDown.Domain.Interfaces;

public interface IUserPhotoProvider
{
    Task<string> UploadPhotoAsync(IFormFile file);
    Task<Result<string>> GetPhotoUrlAsync();
    Task<Result.Result> DeletePhotoAsync();
}