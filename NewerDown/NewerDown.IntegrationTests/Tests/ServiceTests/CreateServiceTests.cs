using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Domain.DTOs.Service;
using NewerDown.IntegrationTests.Helpers;
using NewerDown.IntegrationTests.Services;
using NewerDown.Shared.Validations;

namespace NewerDown.IntegrationTests.Tests.ServiceTests;

[Collection("Test collection")]
public class CreateServiceTests : IAsyncLifetime, IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly AuthenticationService _authenticationService;

    public CreateServiceTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _authenticationService = factory.Services.GetRequiredService<AuthenticationService>();
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _factory.ResetDatabaseAsync();
    
    [Fact]
    public async Task CreateService_ShouldSucceed_WhenRequestIsValid()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var authClient = new AuthenticatedHttpClient(httpClient);
        var authenticatedClient = await authClient.CreateAuthenticatedClientAsync();
        var service = new AddServiceDto()
        {
            Name = "Test Service",
            Url = "https://example.com",
            CheckIntervalSeconds = 2,
            IsActive = true,
            CreatedAt = DateTime.Now,
        };

        // Act
        var httpResponse = await authenticatedClient.PostAsJsonAsync("/api/services", service);

        // Assert
        httpResponse.EnsureSuccessStatusCode();

        var responseBody = await httpResponse.Content.ReadAsStringAsync();
        responseBody.Should().BeNullOrEmpty();
    }
    
    [Fact]
    public async Task CreateService_ShouldFail_WhenRequestIsNotValid()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var authClient = new AuthenticatedHttpClient(httpClient);
        var authenticatedClient = await authClient.CreateAuthenticatedClientAsync();
        var service = new AddServiceDto()
        {
            Url = "https://example.com",
            CheckIntervalSeconds = 2,
            IsActive = true,
            CreatedAt = DateTime.Now,
        };

        // Act
        var httpResponse = await authenticatedClient.PostAsJsonAsync("/api/services", service);

        // Assert
        var responseBody = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        responseBody.Should().NotBeNull();
        responseBody.Errors.Should().ContainKey("Name");
        responseBody.Errors["Name"].First().Should().Be("The Name field is required.");
    }
}