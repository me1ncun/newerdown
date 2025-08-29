/*using System.Net.Http.Json;
using FluentAssertions;
using NewerDown.Domain.DTOs.Notifications;
using NewerDown.Domain.Entities;
using NewerDown.Domain.Enums;
using NewerDown.IntegrationTests.Helpers;
using Monitor = NewerDown.Domain.Entities.Monitor;

namespace NewerDown.IntegrationTests.Tests.NotificationTests;

public class CreateNotificationTests : BaseIntegrationTest
{
    public CreateNotificationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateNotification_ShouldSucceed_WhenRequestIsValid()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        var authClient = new AuthenticatedHttpClient(httpClient);
        var authenticatedClient = await authClient.CreateAuthenticatedClientAsync();
        var currentUserId = await _authenticationService.GetCurrentUserIdAsync(authenticatedClient);
        var service = new Monitor()
        {
            Id = Guid.NewGuid(),
            Name = "Test Service",
            Url = "https://example.com",
            IsActive = true,
            CreatedAt = DateTime.Now,
            UserId = Guid.Parse(currentUserId)
        };
        
        var request = new AddAlertDto
        {
            ServiceId = service.Id,
            Channel = NotificationChannel.PushNotification,
            Target = "new@example.com",
            NotifyOnFailure = false,
            NotifyOnRecovery = true
        };

        // Act
        var postServiceHttpResponse = await authenticatedClient.PostAsJsonAsync("/api/services", service);
        var postRuleHttpResponse = await authenticatedClient.PostAsJsonAsync("/api/notifications/rules", request);

        // Assert
        postServiceHttpResponse.EnsureSuccessStatusCode();
        postRuleHttpResponse.EnsureSuccessStatusCode();

        var responseBody = await postRuleHttpResponse.Content.ReadAsStringAsync();
        responseBody.Should().BeNullOrEmpty();
    }
}*/