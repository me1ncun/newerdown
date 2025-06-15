using System.Collections.Concurrent;
using Azure.Messaging.ServiceBus;

namespace NewerDown.Infrastructure.Queuing;

public class QueueSenderFactory : IQueueSenderFactory
{
    private readonly ServiceBusClient _client;

    public QueueSenderFactory(ServiceBusClient client)
    {
        _client = client;
    }

    public IQueueSender Create(string queueName)
    {
        var sender = _client.CreateSender(queueName);
        return new QueueSender(sender);
    }
}