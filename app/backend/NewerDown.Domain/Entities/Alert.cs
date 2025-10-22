using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.Entities;

public class Alert
{
    public Guid Id { get; set; }
    
    public AlertType Type { get; set; }
    
    public string Target { get; set; } = default!; // Email address or URL
    
    public DateTime CreatedAt { get; set; }
    
    public string? Message { get; set; }

    public Guid MonitorId { get; set; }
    
    public Monitor Monitor { get; set; } = default!;
    

    public DateTime? LastTriggeredAt { get; set; }

    public Guid UserId { get; set; }
    
    public User User { get; set; } = default!;
}

public sealed class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.HasOne(a => a.User)
            .WithMany() 
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Monitor)
            .WithMany(m => m.Alerts)
            .HasForeignKey(a => a.MonitorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}