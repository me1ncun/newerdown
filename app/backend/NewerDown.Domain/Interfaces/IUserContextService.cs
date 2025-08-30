using NewerDown.Domain.DTOs.User;

namespace NewerDown.Domain.Interfaces;

public interface IUserContextService
{
    Guid GetUserId();
    
    Task<UserDto?> GetCurrentUserAsync();
}