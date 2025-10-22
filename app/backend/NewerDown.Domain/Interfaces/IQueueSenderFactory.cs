using NewerDown.Domain.Interfaces;

namespace NewerDown.Infrastructure.Queuing;

public interface IQueueSenderFactory
{
    IQueueSender Create(string queueName);
}