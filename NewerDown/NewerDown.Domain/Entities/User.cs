using Microsoft.AspNetCore.Identity;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
    public Guid? FileAttachmentId { get; set; } 
    public RoleType Role { get; set; }
    public FileAttachment? FileAttachment { get; set; }
    public ICollection<Monitor> Monitors { get; set; } = new List<Monitor>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}