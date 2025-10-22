using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewerDown.Domain.Entities;

public class IncidentComment
{
    public Guid Id { get; set; }

    public Guid IncidentId { get; set; }
    
    public Incident Incident { get; set; } = default!;
    

    public Guid UserId { get; set; }
    
    public User User { get; set; } = default!;
    
    public string Comment { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; }
}

public sealed class IncidentCommentConfiguration : IEntityTypeConfiguration<IncidentComment>
{
    public void Configure(EntityTypeBuilder<IncidentComment> builder)
    {
        builder.HasOne(ic => ic.Incident)
            .WithMany(i => i.Comments) 
            .HasForeignKey(ic => ic.IncidentId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}