using EventManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.Infrastructure.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.Property(e => e.JoinedAt)
            .HasDefaultValueSql("NOW()");

        builder.HasKey(p => new { p.UserId, p.EventId });

        builder
            .HasOne(p => p.User)
            .WithMany(u => u.Participations)
            .HasForeignKey(p => p.UserId);

        builder
            .HasOne(p => p.Event)
            .WithMany(e => e.Participants)
            .HasForeignKey(p => p.EventId);

        builder.HasData(
            new Participant
            {
                Id = Guid.Parse("d2e3f4a5-56b7-4b89-8cde-fedcba654331"),
                UserId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890af"),
                EventId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890aa"),
                JoinedAt = new DateTime(2025, 10, 27, 12, 0, 0, DateTimeKind.Utc)
            },
            new Participant
            {
                Id = Guid.Parse("d2e3f4a5-56b7-4b89-8cde-fedcba654332"),
                UserId = Guid.Parse("d2e3f4a5-56b7-4b89-8cde-fedcba654321"),
                EventId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ab"),
                JoinedAt = new DateTime(2025, 10, 28, 12, 0, 0, DateTimeKind.Utc)
            },
            new Participant
            {
                Id = Guid.Parse("d2e3f4a5-56b7-4b89-8cde-fedcba654333"),
                UserId = Guid.Parse("c1b2e3f4-12d4-4cde-91a7-abcdef123456"),
                EventId = Guid.Parse("b0a0e3f2-46e9-4dc8-83f3-1234567890ac"),
                JoinedAt = new DateTime(2025, 11, 2, 12, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
