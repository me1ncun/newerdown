using System.Reflection;
using NewerDown.Infrastructure.Attributes;
using NewerDown.Infrastructure.Queuing;

namespace NewerDown.Infrastructure.Extensions;

public static class QueueTypeExtensions
{
    public static string GetQueueName(this QueueType queueType)
    {
        var memberInfo = typeof(QueueType).GetMember(queueType.ToString());
        var attribute = memberInfo[0].GetCustomAttribute<QueueNameAttribute>();
        return attribute?.Name ?? queueType.ToString();
    }
}