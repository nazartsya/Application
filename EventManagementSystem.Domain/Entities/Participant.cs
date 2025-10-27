namespace EventManagementSystem.Domain.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = default!;
    public Guid EventId { get; set; }
    public Event Event { get; set; } = default!;
    public DateTime JoinedAt { get; set; }
}
