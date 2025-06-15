using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface IAuthService
{
    string GenerateToken(User user);
}