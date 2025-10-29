namespace EventManagementSystem.Domain.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Event> Events { get; set; } = [];
}
