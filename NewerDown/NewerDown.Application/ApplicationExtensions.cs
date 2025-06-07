using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Application.Validators;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(NotificationRuleMappingProfile).Assembly);
        services.AddValidatorsFromAssembly(typeof(AddServiceDtoValidator).Assembly);

        services.AddSingleton<ICacheService, CacheService>();
        
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<IMonitoringResultService, MonitoringResultService>();
        services.AddScoped<INotificationRuleService, NotificationRuleService>();
        services.AddScoped<IServicesService, ServicesService>();

        return services;
    }
}