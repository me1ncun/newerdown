using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using NewerDown.ServicingFunctions.Options;

namespace NewerDown.ServicingFunctions.Services;

public class EmailService : IEmailSender
{
    private readonly SmtpOptions _smtpOptions;
    
    public EmailService(IOptions<SmtpOptions> smtpOptions)
    {
        _smtpOptions = smtpOptions.Value;
    }
    
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using var body = new TextPart(TextFormat.Html);
        body.Text = htmlMessage;
        
        var message = new MimeMessage
        {
            Subject = subject,
            Body = body
        };
        message.From.Add(new MailboxAddress(_smtpOptions.SenderName, _smtpOptions.SenderEmail));
        message.To.Add(new MailboxAddress(null, email));
        
        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port);
        await client.AuthenticateAsync(_smtpOptions.UserName, _smtpOptions.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}