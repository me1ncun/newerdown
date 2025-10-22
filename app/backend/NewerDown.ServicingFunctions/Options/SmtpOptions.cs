using System.ComponentModel.DataAnnotations;

namespace NewerDown.ServicingFunctions.Options;

public class SmtpOptions
{
    public const string Smtp = "Smtp";
    [Required]
    public string Host { get; set; }
    [Required]
    public int Port { get; set; }
    [Required]
    public string SenderName { get; set; } = string.Empty;
    [Required]
    public string SenderEmail { get; set; } = string.Empty;
    [Required]
    public string UserName { get; set; } = string.Empty;  
    [Required]
    public string Password { get; set; } = string.Empty;
}