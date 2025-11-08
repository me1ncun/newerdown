namespace NewerDown.Domain.DTOs.User;

public class UserDto
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty!;
    
    public Guid? FileAttachmentId { get; set; }
    
    public string FilePath { get; set; } = string.Empty;
    
    public string? OrganizationName { get; set; }
    
    public string? TimeZone { get; set; }
    
    public string? Language { get; set; }
    
    public string? PhoneNumber { get; set; }
}