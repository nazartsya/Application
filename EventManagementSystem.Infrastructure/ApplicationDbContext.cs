using EventManagementSystem.Application.Contracts;
using EventManagementSystem.Domain.Common;
using EventManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EventManagementSystem.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    private readonly ILoggedInUserService? _loggedInUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ILoggedInUserService loggedInUserService)
        : base(options)
    {
        _loggedInUserService = loggedInUserService;
    }

    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Participant> Participants { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _loggedInUserService!.UserId;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = _loggedInUserService!.UserId;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
