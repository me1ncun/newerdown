using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IUserService
{
    Guid GetUserId();
    Task<User?> GetUserByIdAsync(Guid userId);
}