using NewerDown.Domain.Result;

namespace NewerDown.Application.Errors;

public class PhotoErrors
{
    public static readonly Error UserPhotoNotFound = new Error(
        "Photos.UserPhotoNotFound", "User photo not found");
}