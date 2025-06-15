using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Infrastructure.Extensions.DependencyInjection;
using NewerDown.Infrastructure.Queuing;

namespace NewerDown.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceBus(configuration);
        services.AddDataCore(configuration);
        services.AddAuthentication(configuration);
        services.AddIdentity(configuration);

        return services;
    }
}