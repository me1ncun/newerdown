using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NewerDown.Infrastructure.Data;
using NewerDown.IntegrationTests.Helpers;
using NewerDown.IntegrationTests.Services;
using NewerDown.Shared.Interfaces;
using Respawn;
using Testcontainers.MsSql;

namespace NewerDown.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();
    
    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        _dbConnection = new SqlConnection(_dbContainer.GetConnectionString());
        await _dbConnection.OpenAsync();
        
        CreateClient();
        
        await ApplyMigrationsAsync();
        await InitializeRespawnerAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await _dbConnection.DisposeAsync();
    }
    
    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            services.AddDbContext<ApplicationDbContext>((IServiceProvider sp, DbContextOptionsBuilder opts) =>
            {
                opts.UseSqlServer(_dbContainer.GetConnectionString(), (options) =>
                    {
                        options.EnableRetryOnFailure();
                    });
            });
            
            services.AddTransient<AuthenticatedHttpClient>();
            services.AddTransient<AuthenticationService>();
        });
    }
    
    private async Task InitializeRespawnerAsync()
    {
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            SchemasToInclude = [ "dbo" ],
            DbAdapter = DbAdapter.SqlServer
        });
    }
    
    private async Task ApplyMigrationsAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}