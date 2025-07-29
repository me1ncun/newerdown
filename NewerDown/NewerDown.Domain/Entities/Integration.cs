namespace NewerDown.Domain.Entities;

public class Integration
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!; // Slack, Teams, Webhook, etc.
    public string EndpointUrl { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}