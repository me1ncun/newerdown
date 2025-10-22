using NewerDown.Domain.DTOs.Email;

namespace NewerDown.Domain.Interfaces;

public interface IEmailMessageService
{
    EmailMessageDto CreateWelcomeMessage(string userEmail, string userName);
}