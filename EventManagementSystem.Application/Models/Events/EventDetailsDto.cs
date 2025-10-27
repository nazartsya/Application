using EventManagementSystem.Application.Models.Participants;

namespace EventManagementSystem.Application.Models.Events;

public class EventDetailsDto : EventDto
{
    public List<ParticipantDto> Participants { get; set; } = [];
}
