using NewerDown.Application.Time;
using NewerDown.Domain.DTOs.Email;
using NewerDown.Domain.Interfaces;
using NewerDown.Shared.Builders;

namespace NewerDown.Application.Services;

public class EmailMessageService : IEmailMessageService
{
    private readonly IScopedTimeProvider _scopedTimeProvider;

    public EmailMessageService(IScopedTimeProvider scopedTimeProvider)
    {
        _scopedTimeProvider = scopedTimeProvider;
    }
    
    public EmailMessageDto CreateWelcomeMessage(string userEmail, string userName)
    {
        var description = $"Hello, {userName}! 🎉<br/>" +
                          "Thank you for joining our platform. We're excited to have you on board!";
        
        var subject = "NewerDown Welcome message";
        
        var body = new EmailMessageBuilder()
            .SetTitle("Welcome to NewerDown!")
            .SetDescription(description)
            .SetDate(_scopedTimeProvider.UtcNow())
            .BuildHtml();

        return new EmailMessageDto
        {
            Email = userEmail,
            Body = body,
            Subject = subject
        };
    }
}