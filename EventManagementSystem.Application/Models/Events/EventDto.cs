namespace EventManagementSystem.Application.Models.Events;

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public int? Capacity { get; set; }
    public bool IsVisible { get; set; }
    public bool IsJoined { get; set; }
    public int ParticipantsCount { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
