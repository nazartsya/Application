using EventManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.Infrastructure.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Title)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1500)
            .IsRequired();

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.Location)
            .IsRequired();

        builder.Property(e => e.IsVisible)
            .HasDefaultValue(true);

        builder.HasData(
            new Event
            {
                Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890aa"),
                Title = "Tech Conference",
                Description = "A modern tech meetup",
                Location = "Kyiv",
                Date = new DateTime(2025, 10, 31, 9, 0, 0, DateTimeKind.Utc),
                Capacity = 150,
                IsVisible = true,
                CreatedBy = "b0a0e3f2-46e9-4dc8-83f3-1234567890af",
                CreatedDate = new DateTime(2025, 10, 31, 9, 0, 0, DateTimeKind.Utc),
            },
            new Event
            {
                Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ab"),
                Title = "Music Festival",
                Description = "Annual open-air music fest",
                Location = "Lviv",
                Date = new DateTime(2025, 11, 03, 18, 0, 0, DateTimeKind.Utc),
                Capacity = 1000,
                IsVisible = true,
                CreatedBy = "b0a0e3f2-46e9-4dc8-83f3-1234567890af",
                CreatedDate = new DateTime(2025, 10, 31, 9, 0, 0, DateTimeKind.Utc),
            },
            new Event
            {
                Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ac"),
                Title = "Startup Pitch",
                Description = "Pitch your startup ideas",
                Location = "Odessa",
                Date = new DateTime(2025, 11, 6, 14, 0, 0, DateTimeKind.Utc),
                Capacity = 50,
                IsVisible = true,
                CreatedBy = "b0a0e3f2-46e9-4dc8-83f3-1234567890af",
                CreatedDate = new DateTime(2025, 11, 6, 14, 0, 0, DateTimeKind.Utc),
            },
            new Event
            {
                Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ad"),
                Title = "Art Exhibition",
                Description = "Local artists showcase",
                Location = "Kharkiv",
                Date = new DateTime(2025, 11, 9, 11, 0, 0, DateTimeKind.Utc),
                Capacity = 200,
                IsVisible = false,
                CreatedBy = "b0a0e3f2-46e9-4dc8-83f3-1234567890af",
                CreatedDate = new DateTime(2025, 11, 9, 11, 0, 0, DateTimeKind.Utc),
            },
            new Event
            {
                Id = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ae"),
                Title = "Cooking Workshop",
                Description = "Learn to cook like a chef",
                Location = "Dnipro",
                Date = new DateTime(2025, 11, 12, 16, 0, 0, DateTimeKind.Utc),
                Capacity = 30,
                IsVisible = true,
                CreatedBy = "b0a0e3f2-46e9-4dc8-83f3-1234567890af",
                CreatedDate = new DateTime(2025, 11, 12, 16, 0, 0, DateTimeKind.Utc)
            }
        );

        builder
            .HasMany(e => e.Tags)
            .WithMany(t => t.Events)
            .UsingEntity<Dictionary<string, object>>(
                "EventTags",
                j => j.HasOne<Tag>().WithMany().HasForeignKey("TagsId"),
                j => j.HasOne<Event>().WithMany().HasForeignKey("EventsId"),
                j =>
                {
                    j.HasKey("EventsId", "TagsId");
                    j.HasData(
                        new { EventsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890aa"), TagsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ba") },
                        new { EventsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ab"), TagsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890bb") },
                        new { EventsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ac"), TagsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890bc") },
                        new { EventsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ad"), TagsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890bc") },
                        new { EventsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ae"), TagsId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890bc") }
                    );
                }
            );
    }
}
