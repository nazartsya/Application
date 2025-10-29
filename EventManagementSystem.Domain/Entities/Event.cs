using EventManagementSystem.Domain.Common;

namespace EventManagementSystem.Domain.Entities;

public class Event : AuditableEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime Date { get; set; }
    public required string Location { get; set; }
    public int? Capacity { get; set; }
    public bool IsVisible { get; set; }
    public ICollection<Participant> Participants { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
}
