using EventManagementSystem.Application.Models.Events;
using Microsoft.AspNetCore.JsonPatch;

namespace EventManagementSystem.Application.Contracts.Services;

public interface IEventService
{
    Task<IEnumerable<EventDto>> GetEventsAsync(Guid? userId, IEnumerable<string>? tagNames);
    Task<EventDetailsDto> GetEventAsync(Guid eventId, Guid userId);
    Task<EventDto> CreateEventAsync(EventForCreationDto dto);
    Task PartiallyUpdateEventAsync(Guid eventId, JsonPatchDocument<EventForUpdateDto> patchDoc);
    Task DeleteEventAsync(Guid eventId);
    Task JoinEventAsync(Guid eventId, Guid userId);
    Task LeaveEventAsync(Guid eventId, Guid userId);
}
