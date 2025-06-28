using NewerDown.Infrastructure.Attributes;

namespace NewerDown.Infrastructure.Queuing;

public enum QueueType
{
    [QueueName("emails")]
    Emails,
    [QueueName("notifications")]
    Notifications
}