using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewerDown.Domain.Enums;

namespace NewerDown.Domain.Entities;

public class Monitor
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Target { get; set; } = default!;
    
    public int? Port { get; set; }
    
    public MonitorType Type { get; set; }
    
    public int IntervalSeconds { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Guid? UserId { get; set; }
    
    public User User { get; set; }
    
    public ICollection<MonitorCheck> Checks { get; set; } = new List<MonitorCheck>();
    
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}

public sealed class MonitorConfiguration : IEntityTypeConfiguration<Monitor>
{
    public void Configure(EntityTypeBuilder<Monitor> builder)
    {
        builder.HasOne(m => m.User)
            .WithMany(u => u.Monitors)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}