using Microsoft.AspNetCore.Identity;
using NewerDown.Domain.DTOs.Account;
using NewerDown.Domain.Entities;

namespace NewerDown.Domain.Interfaces;

public interface ISignInService
{
    Task<string> LoginUserAsync(LoginUserDto request);
    Task ChangePasswordAsync(ChangePasswordDto request);
    Task RegisterUserAsync(RegisterUserDto request);
}