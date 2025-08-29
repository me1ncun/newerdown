using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace NewerDown.Infrastructure.Extensions;

public static class MessagingExtensions
{
    public static ServiceBusMessage SetBody<T>(this ServiceBusMessage message, T value)
    {
        message.Body = new BinaryData(value);

        return message;
    }
    
    public static T GetBody<T>(this ServiceBusReceivedMessage message)
    {
        var jsonString = message.Body.ToString(); 
        return JsonSerializer.Deserialize<T>(jsonString); 
    }
}