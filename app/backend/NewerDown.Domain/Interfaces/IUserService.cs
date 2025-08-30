using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IUserService
{
    Guid GetUserId();
    
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    
    Task<UserDto?> GetCurrentUserAsync();
    
    Task<User?> GetUserByIdAsync(Guid userId);
    
    Task DeleteUserAsync();
    
    Task<UserDto> UpdateUserAsync(UpdateUserDto request);
}