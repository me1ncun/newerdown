using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
    public string? OrganizationName { get; set; }
    
    public SubscriptionPlanType? SubscriptionPlan { get; set; } 
    
    public DateTime? SubscriptionExpiresAt { get; set; }
    
    public string? TimeZone { get; set; }
    
    public string? Language { get; set; }
    
    public Guid? FileAttachmentId { get; set; } 
    
    public RoleType Role { get; set; }
    
    public FileAttachment? FileAttachment { get; set; }
    
    public ICollection<Monitor> Monitors { get; set; } = new List<Monitor>();
    
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.HasOne(u => u.FileAttachment)
            .WithMany()
            .HasForeignKey(u => u.FileAttachmentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}