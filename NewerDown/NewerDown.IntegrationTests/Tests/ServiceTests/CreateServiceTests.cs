using System.Net.Http.Json;
using FluentAssertions;
using NewerDown.Domain.DTOs.Service;
using NewerDown.IntegrationTests.Helpers;
using NewerDown.Shared.Validations;

namespace NewerDown.IntegrationTests.Tests.ServiceTests;

public class CreateServiceTests : BaseIntegrationTest
{
    public CreateServiceTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

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
        responseBody.Should().NotBeNull();
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