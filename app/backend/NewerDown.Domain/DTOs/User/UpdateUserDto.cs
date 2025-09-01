namespace NewerDown.Domain.DTOs.User;

public class UpdateUserDto
{
    public string? UserName { get; set; }
    
    public string? Email { get; set; }
    
    public string? OrganizationName { get; set; }
    
    public string? TimeZone { get; set; }
    
    public string? Language { get; set; }
    
    public string? DisplayName { get; set; }
    
    public string? PhoneNumber { get; set; }
}