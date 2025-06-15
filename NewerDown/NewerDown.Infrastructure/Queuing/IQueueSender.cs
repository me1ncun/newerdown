namespace NewerDown.Infrastructure.Queuing;

public interface IQueueSender
{
    Task SendAsync<T>(T value, Guid sessionId = default, string contentType = null, bool useXml = false);
}