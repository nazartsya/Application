using EventManagementSystem.Application.Contracts.Repositories;
using EventManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ITagRepository _tagRepository;

    public EventRepository(ApplicationDbContext context, ITagRepository tagRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(IEnumerable<string>? tagNames = null)
    {
        var query = _context.Events
            .Include(e => e.Participants)
            .Include(e => e.Tags)
            .AsQueryable();

        if (tagNames != null && tagNames.Any())
        {
            query = query.Where(e => e.Tags.Any(t => tagNames.Contains(t.Name)));
        }

        return await query.ToListAsync();
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
            .Include(e => e.Tags)
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

    public async Task AddEvent(Event @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        @event.Id = Guid.NewGuid();

        await HandleTagsAsync(@event);

        _context.Events.Add(@event);
    }

    public async Task UpdateEvent(Event @event)
    {
        await HandleTagsAsync(@event);
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

    private async Task HandleTagsAsync(Event @event)
    {
        var tagNames = @event.Tags
            .Select(t => t.Name.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var existingTags = await _tagRepository.GetTagsByNamesAsync(tagNames);

        var newTagNames = tagNames
            .Except(existingTags.Select(t => t.Name), StringComparer.OrdinalIgnoreCase)
            .ToList();

        var newTags = newTagNames.Select(n => new Tag { Id = Guid.NewGuid(), Name = n }).ToList();

        @event.Tags = existingTags.Concat(newTags).ToList();
    }
}
