using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.Entities;
using Monitor = NewerDown.Domain.Entities.Monitor;

namespace NewerDown.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Monitor> Monitors { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<IncidentComment> IncidentComments { get; set; }
    public DbSet<Integration> Integrations { get; set; }
    public DbSet<MonitorCheck> MonitorChecks { get; set; }
    public DbSet<FileAttachment> FileAttachments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Alert>()
            .HasOne(a => a.User)
            .WithMany() 
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
        
        modelBuilder.Entity<Alert>()
            .HasOne(a => a.Monitor)
            .WithMany(m => m.Alerts)
            .HasForeignKey(a => a.MonitorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<IncidentComment>()
            .HasOne(ic => ic.Incident)
            .WithMany(i => i.Comments) 
            .HasForeignKey(ic => ic.IncidentId)
            .OnDelete(DeleteBehavior.Restrict); 
        
       
        modelBuilder.Entity<Incident>()
            .HasOne(i => i.Monitor)
            .WithMany()
            .HasForeignKey(i => i.MonitorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Monitor>()
            .HasOne(m => m.User)
            .WithMany(u => u.Monitors)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Integration>()
            .HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.FileAttachment)
            .WithMany()
            .HasForeignKey(u => u.FileAttachmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}