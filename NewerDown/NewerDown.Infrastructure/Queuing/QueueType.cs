using System.Reflection;

namespace NewerDown.Infrastructure.Queuing;

public enum QueueType
{
    [QueueName("emails")]
    Emails,
    [QueueName("notifications")]
    Notifications
}

public class QueueNameAttribute : Attribute
{
    public QueueNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public static class QueueTypeExtensions
{
    public static string GetQueueName(this QueueType queueType)
    {
        var memberInfo = typeof(QueueType).GetMember(queueType.ToString());
        var attribute = memberInfo[0].GetCustomAttribute<QueueNameAttribute>();
        return attribute?.Name ?? queueType.ToString();
    }
}