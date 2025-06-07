using Microsoft.AspNetCore.Identity;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Exceptions;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application.Services;

public class SignInService : ISignInService
{
    private readonly UserManager<User> _userManager;

    public SignInService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task ChangePassword(string? userId, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null)
        {
            throw new EntityNotFoundException("User not found.");
        }

        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception($"Password change failed: {errors}");
        }
    }
}