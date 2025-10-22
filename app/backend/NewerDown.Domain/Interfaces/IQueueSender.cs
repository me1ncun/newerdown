namespace NewerDown.Domain.Interfaces;

public interface IQueueSender
{
    Task SendAsync<T>(T value, Guid sessionId = default, string contentType = null, bool useXml = false);
}