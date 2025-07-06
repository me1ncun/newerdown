using Microsoft.AspNetCore.Identity;
using NewerDown.Application;
using NewerDown.Domain.Entities;
using NewerDown.Extensions;
using NewerDown.Infrastructure;
using NewerDown.Infrastructure.Data;
using NewerDown.Infrastructure.Extensions;
using NewerDown.Middlewares;
using NewerDown.Shared;
using Microsoft.Extensions.Configuration;

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
        services.AddLogging(builder =>
        {
            builder.AddApplicationInsights(Configuration["ApplicationInsightsInstrumentationKey"]);
        });
        services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = Configuration["ApplicationInsightsConnection"];
        });
        
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddSwaggerDocumentation();
        services.AddCORS();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSignalR().AddAzureSignalR(Configuration["SignalRConnection"]);;
        
        services.AddDistributedMemoryCache();
        
        services.AddHttpContextAccessor();

        services.AddSharedServices(Configuration);
        services.AddApplicationServices(Configuration);
        services.AddInfrastructureServices(Configuration);

        services.AddHttpClient();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsProduction())
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