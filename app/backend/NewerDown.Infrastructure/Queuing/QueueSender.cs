using Azure.Messaging.ServiceBus;
using NewerDown.Domain.Interfaces;
using NewerDown.Infrastructure.Extensions;

namespace NewerDown.Infrastructure.Queuing;

public class QueueSender : IQueueSender
{
    private readonly ServiceBusSender _senderClient;

    public QueueSender(ServiceBusSender senderClient)
    {
        _senderClient = senderClient;
    }

    public Task SendAsync<T>(T value, Guid sessionId = default, string contentType = null, bool useXml = false)
    {
        var message = CreateMessage(value, sessionId, contentType, useXml);

        return _senderClient.SendMessageAsync(message);
    }

    private ServiceBusMessage CreateMessage<T>(T value, Guid sessionId, string contentType, bool useXml = false)
    {
        var message = new ServiceBusMessage
        {
            ContentType = contentType
        };

        message.SetBody(value);

        if (sessionId != default)
        {
            message.SessionId = sessionId.ToString();
        }

        return message;
    }
}