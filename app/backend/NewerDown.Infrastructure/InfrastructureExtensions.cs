using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Extensions.DependencyInjection;
using NewerDown.Infrastructure.Queuing;
using NewerDown.Infrastructure.Webhooks;

namespace NewerDown.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceBus(configuration);
        services.AddDataCore(configuration);
        services.AddAuthentication(configuration);
        services.AddIdentity(configuration);
        
        services.AddScoped<IWebhookSender, WebhookSender>();

        return services;
    }
}