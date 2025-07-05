using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using NewerDown.Domain.DTOs.Service;
using NewerDown.IntegrationTests.Helpers;
using NewerDown.Shared.Validations;

namespace NewerDown.IntegrationTests.Tests.ServiceTests;


public class UpdateServiceTests : BaseIntegrationTest
{
    public UpdateServiceTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

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
            IsActive = true,
        };
        
        var addService = new AddServiceDto()
        {
            Name = "Test Service",
            Url = "https://example.com",
            IsActive = true,
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
            IsActive = true,
        };
        
        var addService = new AddServiceDto()
        {
            Name = "Test Service",
            Url = "https://example.com",
            IsActive = true,
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