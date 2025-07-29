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
}