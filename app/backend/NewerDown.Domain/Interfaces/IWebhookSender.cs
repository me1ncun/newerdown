namespace NewerDown.Domain.Interfaces;

public interface IWebhookSender
{
    Task<bool> SendAsync(string url, object payload, CancellationToken cancellationToken = default);
}