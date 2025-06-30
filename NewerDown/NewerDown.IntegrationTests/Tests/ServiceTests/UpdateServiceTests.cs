using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Domain.DTOs.Service;
using NewerDown.IntegrationTests.Helpers;
using NewerDown.IntegrationTests.Services;
using NewerDown.Shared.Validations;

namespace NewerDown.IntegrationTests.Tests.ServiceTests;

[Collection("Test collection")]
public class UpdateServiceTests : IAsyncLifetime, IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly AuthenticationService _authenticationService;

    public UpdateServiceTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _authenticationService = factory.Services.GetRequiredService<AuthenticationService>();
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _factory.ResetDatabaseAsync();
    
    [Fact]
    public async Task UpdateService_ShouldSucceed_WhenRequestIsValid()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var authClient = new AuthenticatedHttpClient(httpClient);
        var authenticatedClient = await authClient.CreateAuthenticatedClientAsync();
        var updateService = new UpdateServiceDto()
        {
            Name = "Test Service",
            Url = "https://example.com",
            CheckIntervalSeconds = 2,
            IsActive = true,
            CreatedAt = DateTime.Now,
        };
        
        var addService = new AddServiceDto()
        {
            Name = "Test Service",
            Url = "https://example.com",
            CheckIntervalSeconds = 2,
            IsActive = true,
            CreatedAt = DateTime.Now,
        };
        
        var request = JsonSerializer.Serialize(updateService);
        var content = new StringContent(request, System.Text.Encoding.UTF8, "application/json");

        // Act
        var postResponse = await authenticatedClient.PostAsJsonAsync("/api/services", addService);
        var createdId = await postResponse.Content.ReadFromJsonAsync<Guid>();
        
        var httpResponse = await authenticatedClient.PutAsync($"/api/services/{createdId}", content);

        // Assert
        var result = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();

        var responseBody = await httpResponse.Content.ReadAsStringAsync();
        responseBody.Should().BeNullOrEmpty();
    }
    
    [Fact]
    public async Task UpdateService_ShouldFail_WhenRequestIsNotValid()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var authClient = new AuthenticatedHttpClient(httpClient);
        var authenticatedClient = await authClient.CreateAuthenticatedClientAsync();
        var updateService = new UpdateServiceDto()
        {
            Url = "https://example.com",
            CheckIntervalSeconds = 2,
            IsActive = true,
            CreatedAt = DateTime.Now,
        };
        
        var addService = new AddServiceDto()
        {
            Name = "Test Service",
            Url = "https://example.com",
            CheckIntervalSeconds = 2,
            IsActive = true,
            CreatedAt = DateTime.Now,
        };
        
        var request = JsonSerializer.Serialize(updateService);
        var content = new StringContent(request, System.Text.Encoding.UTF8, "application/json");

        // Act
        var postResponse = await authenticatedClient.PostAsJsonAsync("/api/services", addService);
        var createdId = await postResponse.Content.ReadFromJsonAsync<Guid>();
        
        var httpResponse = await authenticatedClient.PutAsync($"/api/services/{createdId}", content);

        // Assert
        var responseBody = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        responseBody.Should().NotBeNull();
        responseBody.Errors.Should().ContainKey("Name");
        responseBody.Errors["Name"].First().Should().Be("The Name field is required.");
    }
}