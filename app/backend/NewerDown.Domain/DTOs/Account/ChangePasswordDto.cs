namespace NewerDown.Domain.DTOs.Account;

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}