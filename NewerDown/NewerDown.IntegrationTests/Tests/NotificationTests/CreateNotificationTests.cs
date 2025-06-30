using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NewerDown.Domain.DTOs.Notifications;
using NewerDown.Domain.Entities;
using NewerDown.IntegrationTests.Helpers;
using NewerDown.IntegrationTests.Services;

namespace NewerDown.IntegrationTests.Tests.NotificationTests;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<CustomWebApplicationFactory>;

[Collection("Test collection")]
public class CreateNotificationTests : IAsyncLifetime, IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly AuthenticationService _authenticationService;

    public CreateNotificationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _authenticationService = factory.Services.GetRequiredService<AuthenticationService>();
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _factory.ResetDatabaseAsync();
    
    [Fact]
    public async Task CreateNotification_ShouldSucceed_WhenRequestIsValid()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var authClient = new AuthenticatedHttpClient(httpClient);
        var authenticatedClient = await authClient.CreateAuthenticatedClientAsync();
        var currentUserId = await _authenticationService.GetCurrentUserIdAsync(authenticatedClient);
        var service = new Service()
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Url = "https://example.com",
            CheckIntervalSeconds = 2,
            IsActive = true,
            CreatedAt = DateTime.Now,
            UserId = Guid.Parse(currentUserId)
        };
        
        var request = new AddNotificationRuleDto
        {
            ServiceId = service.Id,
            Channel = NotificationChannel.PushNotification,
            Target = "new@example.com",
            NotifyOnFailure = false,
            NotifyOnRecovery = true
        };

        // Act
        var response = await authenticatedClient.PostAsJsonAsync("/api/services", service);
        var httpResponse = await authenticatedClient.PostAsJsonAsync("/api/notifications/rules", request);

        // Assert
        var result = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();

        var responseBody = await httpResponse.Content.ReadAsStringAsync();
        responseBody.Should().BeNullOrEmpty();
    }
}