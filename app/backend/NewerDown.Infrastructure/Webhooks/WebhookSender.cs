using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Infrastructure.Webhooks;

public class WebhookSender : IWebhookSender
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WebhookSender> _logger;

    public WebhookSender(HttpClient httpClient, ILogger<WebhookSender> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> SendAsync(string url, object payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Webhook sent successfully to {Url}", url);
                return true;
            }

            _logger.LogWarning("Webhook to {Url} failed with status {StatusCode}", url, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending webhook to {Url}", url);
            return false;
        }
    }
}