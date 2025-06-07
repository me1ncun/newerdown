using Azure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NewerDown.Application;
using NewerDown.Domain.Entities;
using NewerDown.Infrastructure;
using NewerDown.Infrastructure.Data;
using NewerDown.Infrastructure.Extensions;
using NewerDown.Middlewares;
using NewerDown.Shared;

namespace NewerDown;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("http://example.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
        
        services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddAspNetIdentity<User>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSignalR().AddAzureSignalR(Configuration["SignalRConnection"]);;
        
        services.AddDistributedMemoryCache();
        
        services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = Configuration["ApplicationInsightsConnection"];
        });

        services.AddSharedServices(Configuration);
        services.AddApplicationServices(Configuration);
        services.AddInfrastructureServices(Configuration);

        services.AddHttpClient();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.ApplyMigrations();
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseIdentityServer();
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}