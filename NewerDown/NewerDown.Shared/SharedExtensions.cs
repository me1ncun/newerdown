using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Shared.Validations;

namespace NewerDown.Shared;

public static class SharedExtensions
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFluentValidator, FluentValidator>();

        return services;
    }
}