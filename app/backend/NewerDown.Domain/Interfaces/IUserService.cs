using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    
    Task<User?> GetUserByIdAsync(Guid userId);
    
    Task DeleteUserAsync();
    
    Task<UserDto> UpdateUserAsync(UpdateUserDto request);
}