using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Result;

namespace NewerDown.Domain.Interfaces;

public interface IUserContextService
{
    Guid GetUserId();

    Task<Result<UserDto>> GetCurrentUserAsync();
}