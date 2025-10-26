using EventManagementSystem.Domain.Entities;

namespace EventManagementSystem.Application.Contracts.Repositories;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetEventsAsync();
    Task<Event?> GetEventAsync(Guid eventId);
    Task<Event?> GetEventWithParticipantsAsync(Guid eventId);
    void AddEvent(Event @event);
    void UpdateEvent(Event @event);
    void DeleteEvent(Event @event);
    Task<Participant?> GetParticipantAsync(Guid eventId, Guid userId);
    void JoinEvent(Participant participant);
    void LeaveEvent(Participant participant);
    Task<bool> SaveAsync();
}
