using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewerDown.Domain.Entities;

namespace NewerDown.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Service> Services { get; set; }
    public DbSet<MonitoringResult> MonitoringResults { get; set; }
    public DbSet<NotificationRule> NotificationRules { get; set; }
    public DbSet<FileAttachment> FileAttachments { get; set; }
}