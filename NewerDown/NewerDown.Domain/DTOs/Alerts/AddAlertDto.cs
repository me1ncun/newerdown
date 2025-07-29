using NewerDown.Domain.Enums;

namespace NewerDown.Domain.DTOs.Alerts;

public class AddAlertDto
{
    public AlertType Type { get; set; }
    public string Target { get; set; } = default!; // Email address or URL

    public Guid MonitorId { get; set; }
}