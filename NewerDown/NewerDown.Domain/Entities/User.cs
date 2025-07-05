using Microsoft.AspNetCore.Identity;

namespace NewerDown.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
    public List<Service> Services { get; set; }
    public List<NotificationRule> NotificationRules { get; set; }
    public Guid? FileAttachmentId { get; set; }
    public FileAttachment FileAttachment { get; set; }
}