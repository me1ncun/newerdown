using NewerDown.Domain.Entities;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Notification;

public class NotificationDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public AlertType AlertType { get; set; }
    public string Message { get; set; }
    public string Target { get; set; }
}