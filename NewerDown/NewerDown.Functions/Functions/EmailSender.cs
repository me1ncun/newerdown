using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NewerDown.Domain.DTOs.Email;
using NewerDown.Functions.Services;
using NewerDown.Infrastructure.Extensions;

namespace NewerDown.Functions.Functions;

public class EmailSender
{
    private readonly ILogger<EmailSender> _logger;
    private readonly IEmailService _emailService;
    
    public EmailSender(ILogger<EmailSender> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }
    
    [Function(nameof(EmailSender))]
    public async Task Run([ServiceBusTrigger("emails", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
    {
       var email = message.GetBody<EmailDto>();
       
       await _emailService.SendEmailAsync(email.EmailAddress, "NewerDown Registration Confirmation");
       
       _logger.LogInformation("Processing email for {EmailAddress}", email.EmailAddress);
    }
}