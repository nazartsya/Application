using EventManagementSystem.Application.Models.Tags;
using System.Text.Json.Serialization;

namespace EventManagementSystem.Application.Models.Events;

public class EventForCreationDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public int? Capacity { get; set; }
    public bool IsVisible { get; set; }

    [JsonRequired]
    public ICollection<TagForCreationDto> Tags { get; set; } = [];
}
