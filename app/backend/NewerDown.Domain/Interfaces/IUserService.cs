using NewerDown.Domain.DTOs.User;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<Result.Result> DeleteUserAsync();
    Task<Result.Result<UserDto>> UpdateUserAsync(UpdateUserDto request);
}