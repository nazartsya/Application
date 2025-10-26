using EventManagementSystem.Application.Contracts.Repositories;
using EventManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Event>> GetEventsAsync()
    {
        return await _context.Events
            .Include(e => e.Participants)
            .ToListAsync();
    }

    public async Task<Event?> GetEventAsync(Guid eventId)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(eventId));
        }

        return await _context.Events
            .Include(e => e.Participants)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task<Event?> GetEventWithParticipantsAsync(Guid eventId)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(eventId));
        }

        return await _context.Events
            .Include(e => e.Participants)
            .FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public void AddEvent(Event @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        @event.Id = Guid.NewGuid();

        _context.Events.Add(@event);
    }

    public void UpdateEvent(Event @event)
    {
    }

    public void DeleteEvent(Event @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        _context.Events.Remove(@event);
    }

    public async Task<Participant?> GetParticipantAsync(Guid eventId, Guid userId)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(eventId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        return await _context.Participants
            .FirstOrDefaultAsync(p => p.EventId == eventId && p.UserId == userId);
    }

    public void JoinEvent(Participant participant)
    {
        ArgumentNullException.ThrowIfNull(participant);

        participant.Id = Guid.NewGuid();

        _context.Participants.Add(participant);
    }

    public void LeaveEvent(Participant participant)
    {
        ArgumentNullException.ThrowIfNull(participant);

        _context.Participants.Remove(participant);
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}
