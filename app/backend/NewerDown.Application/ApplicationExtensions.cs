using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Application.MappingProfiles;
using NewerDown.Application.Services;
using NewerDown.Application.Time;
using NewerDown.Application.Validators;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddValidatorsFromAssembly(typeof(AddServiceValidator).Assembly);

        services.AddSingleton<ICacheService, CacheService>();
        
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IMonitorService, MonitorService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IIncidentService, IncidentService>();
        services.AddScoped<IAdminService, AdminService>();
        
        services.AddScoped<IUserPhotoProvider, UserPhotoProvider>();

        services.AddScoped<IScopedTimeProvider, ScopedTimeProvider>();

        return services;
    }
}