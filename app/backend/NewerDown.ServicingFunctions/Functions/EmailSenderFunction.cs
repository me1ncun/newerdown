using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NewerDown.Domain.DTOs.Email;
using NewerDown.Domain.DTOs.Service;
using NewerDown.ServicingFunctions.Services;

namespace NewerDown.ServicingFunctions.Functions;

public class EmailSenderFunction
{
    private readonly ILogger<EmailSenderFunction> _logger;
    private readonly IEmailSender _emailService;

    public EmailSenderFunction(
        ILogger<EmailSenderFunction> logger,
        IEmailSender emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    [Function(nameof(EmailSenderFunction))]
    public async Task Run(
        [ServiceBusTrigger("emails", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var req = Encoding.UTF8.GetString(message.Body);
        var emailMessage = JsonSerializer.Deserialize<EmailMessageDto>(req) ??
                      throw new InvalidOperationException("Invalid email message");
        
        await _emailService.SendEmailAsync(emailMessage.Email, emailMessage.Subject, emailMessage.Body);
        _logger.LogInformation("Successfully sent message to  user email: {userEmail}", emailMessage.Email);
    }
}