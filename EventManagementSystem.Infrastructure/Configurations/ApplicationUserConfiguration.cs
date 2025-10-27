using EventManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.Infrastructure.Configurations;
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsRequired();

        var admin = new ApplicationUser
        {
            Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890af"),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true,
            SecurityStamp = "6178f680-6054-4fd9-afbb-f7751abd7687",
            ConcurrencyStamp = "d2d6c6aa-5f8b-4f79-b2fa-5a7c1b0d0e14",
            PasswordHash = "AQAAAAIAAYagAAAAEDB93DmsKz0wh/akFc4jI/jXIQYbfL431njm99H3NMBR3nYZ7UVMuGLYe8uOB4nAbA=="
        };

        var manager = new ApplicationUser
        {
            Id = Guid.Parse("c1b2e3f4-12d4-4cde-91a7-abcdef123456"),
            UserName = "manager",
            NormalizedUserName = "MANAGER",
            Email = "manager@example.com",
            NormalizedEmail = "MANAGER@EXAMPLE.COM",
            FirstName = "Manager",
            LastName = "User",
            EmailConfirmed = true,
            SecurityStamp = "6178f680-6054-4fd9-afbb-f7751abd7687",
            ConcurrencyStamp = "d2d6c6aa-5f8b-4f79-b2fa-5a7c1b0d0e14",
            PasswordHash = "AQAAAAIAAYagAAAAENKlk8l5t8MEVrtQg+9a9hOVHJo4qR0SzkO0dmZvRhu1Rfyf7hMrNut3tuaKZbGMcQ=="
        };

        var user = new ApplicationUser
        {
            Id = Guid.Parse("d2e3f4a5-56b7-4b89-8cde-fedcba654321"),
            UserName = "user",
            NormalizedUserName = "USER",
            Email = "user@example.com",
            NormalizedEmail = "USER@EXAMPLE.COM",
            FirstName = "Regular",
            LastName = "User",
            EmailConfirmed = true,
            SecurityStamp = "6178f680-6054-4fd9-afbb-f7751abd7687",
            ConcurrencyStamp = "d2d6c6aa-5f8b-4f79-b2fa-5a7c1b0d0e14",
            PasswordHash = "AQAAAAIAAYagAAAAECvOnB86eSHIJef9TAHADa7aDBvRe5s1NK9re2QQvt7x4zEUxgv0giiryEX+b3/PFg=="
        };

        builder.HasData(admin, manager, user);
    }
}
