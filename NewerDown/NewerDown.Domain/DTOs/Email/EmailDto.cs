namespace NewerDown.Domain.DTOs.Email;

public class EmailDto
{
    public EmailDto(string emailAddress, string userName, DateTime receivedDate)
    {
        EmailAddress = emailAddress;
        UserName = userName;
        RegisteredAt = receivedDate;
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; set; }
    public string EmailAddress { get; set; }
    public string UserName { get; set; }
    public DateTime RegisteredAt { get; set; }
}