using Microsoft.Extensions.DependencyInjection;
using NewerDown.IntegrationTests.Services;

namespace NewerDown.IntegrationTests.Tests;

[Collection("Test collection")]
public class BaseIntegrationTest : IAsyncLifetime, IClassFixture<CustomWebApplicationFactory>
{
    protected readonly CustomWebApplicationFactory _factory;
    protected readonly AuthenticationService _authenticationService;

    public BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _authenticationService = factory.Services.GetRequiredService<AuthenticationService>();
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _factory.ResetDatabaseAsync();
}