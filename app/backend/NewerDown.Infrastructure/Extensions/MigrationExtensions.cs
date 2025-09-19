using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Infrastructure.Data;
using NewerDown.Infrastructure.Helpers;

namespace NewerDown.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
        
        RoleInitializer.SeedRolesAsync(scope.ServiceProvider).GetAwaiter().GetResult();
    }
}