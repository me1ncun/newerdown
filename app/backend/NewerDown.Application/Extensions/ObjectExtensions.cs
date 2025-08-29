using NewerDown.Domain.Exceptions;

namespace NewerDown.Application.Extensions;

public static class ObjectExtensions
{
    public static T ThrowIfNull<T>(this T? obj, string? message = null) where T : class
    {
        if (obj == null)
            throw new EntityNotFoundException($"{message ?? "he requested entity"} was not found.");

        return obj;
    }
}