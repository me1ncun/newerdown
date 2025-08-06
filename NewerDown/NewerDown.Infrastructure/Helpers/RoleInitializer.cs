using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Domain.Enums;

namespace NewerDown.Infrastructure.Helpers;

public static class RoleInitializer
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        foreach (var role in Enum.GetNames(typeof(RoleType)))
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}