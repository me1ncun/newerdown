using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

public sealed class IntegrationConfiguration : IEntityTypeConfiguration<Integration>
{
    public void Configure(EntityTypeBuilder<Integration> builder)
    {
        builder
            .HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}