using EventManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.Infrastructure.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
            .HasIndex(t => t.Name)
            .IsUnique();

        builder.HasData(
            new Tag
            {
                Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ba"),
                Name = "Technology",
            },
            new Tag
            {
                Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890bb"),
                Name = "Art",
            },
            new Tag
            {
                Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890bc"),
                Name = "Business",
            },
            new Tag
            {
                Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890bd"),
                Name = "Music",
            }
        );
    }
}
