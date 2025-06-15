using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NewerDown.Functions.Builders;
using NewerDown.Functions.Models;

namespace NewerDown.Functions.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject)
    {
        var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
        {
            Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password),
            EnableSsl = _emailSettings.EnableSsl
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
            Subject = subject,
            Body = EmailMessageBuilder.BuildEmailMessage(subject, toEmail),
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}