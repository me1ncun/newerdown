using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Infrastructure.Data;

namespace NewerDown.Infrastructure.Extensions.DependencyInjection;

public static class ServicesExtensions
{
    public static IServiceCollection AddDataCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseAzureSql(configuration["DatabaseConnection"]);
        });

        return services;
    }
}