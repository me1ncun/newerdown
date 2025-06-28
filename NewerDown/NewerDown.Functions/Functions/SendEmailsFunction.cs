using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NewerDown.Domain.DTOs.Email;
using NewerDown.Functions.Services;
using NewerDown.Infrastructure.Extensions;

namespace NewerDown.Functions.Functions;

public class SendEmailsFunction
{
    private readonly ILogger<SendEmailsFunction> _logger;
    private readonly IEmailService _emailService;
    
    public SendEmailsFunction(ILogger<SendEmailsFunction> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    [Function(nameof(SendEmailsFunction))]
    public async Task Run([ServiceBusTrigger("emails", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
    {
       var email = message.GetBody<EmailDto>();
       
       await _emailService.SendEmailAsync(email.EmailAddress, "NewerDown Registration Confirmation");
       
       _logger.LogInformation("Processing email for {EmailAddress}", email.EmailAddress);
    }
}