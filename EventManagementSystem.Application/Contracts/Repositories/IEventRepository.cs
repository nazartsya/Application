using EventManagementSystem.Domain.Entities;

namespace EventManagementSystem.Application.Contracts.Repositories;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetEventsAsync(IEnumerable<string>? tagNames);
    Task<Event?> GetEventAsync(Guid eventId);
    Task<Event?> GetEventWithParticipantsAsync(Guid eventId);
    Task AddEvent(Event @event);
    Task UpdateEvent(Event @event);
    void DeleteEvent(Event @event);
    Task<Participant?> GetParticipantAsync(Guid eventId, Guid userId);
    void JoinEvent(Participant participant);
    void LeaveEvent(Participant participant);
    Task<bool> SaveAsync();
}
