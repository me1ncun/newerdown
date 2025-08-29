using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Alerts;

public class AlertDto
{
    public Guid Id { get; set; }
    public AlertType Type { get; set; }
    public string Target { get; set; } = default!; // Email address or URL

    public Guid MonitorId { get; set; }

    public DateTime? LastTriggeredAt { get; set; }

    public Guid UserId { get; set; }
}