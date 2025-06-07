using Microsoft.AspNetCore.Identity;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface ISignInService
{
    Task ChangePassword(string? userId, string oldPassword, string newPassword);
}