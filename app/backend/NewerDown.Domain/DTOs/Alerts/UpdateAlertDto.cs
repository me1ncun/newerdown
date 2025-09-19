using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Alerts;

public class UpdateAlertDto
{
    public AlertType Type { get; set; }
    
    public string Target { get; set; } = default!; 
    
    public string? Message { get; set; }
}