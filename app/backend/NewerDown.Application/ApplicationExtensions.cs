using System.Reflection;
using FluentValidation;
using GraphQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Application.GraphQL.Mutations;
using NewerDown.Application.GraphQL.Schemas;
using NewerDown.Application.Services;
using NewerDown.Application.Time;
using NewerDown.Domain.DTOs.Service;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddValidatorsFromAssembly(typeof(AddMonitorDtoValidator).Assembly);
        services.AddGraphQLServices();

        services.AddSingleton<ICacheService, CacheService>();
        
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IMonitorService, MonitorService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailMessageService, EmailMessageService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IIncidentService, IncidentService>();
        
        services.AddScoped<IUserPhotoProvider, UserPhotoProvider>();
        services.AddScoped(provider => new Lazy<IUserPhotoProvider>(provider.GetRequiredService<IUserPhotoProvider>));
        services.AddScoped<IScopedTimeProvider, ScopedTimeProvider>();

        return services;
    }
    
    [Obsolete("Obsolete")]
    private static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        services.AddScoped<AppSchema>();

        services.AddGraphQL(builder => builder
            .AddSystemTextJson()
            .AddGraphTypes(Assembly.GetAssembly(typeof(AppMutation)))
            .ConfigureExecutionOptions(options =>
            {
                options.EnableMetrics = true;
            })
        );

        return services;
    }
}