using EventManagementSystem.Application.Contracts;
using EventManagementSystem.Application.Contracts.Repositories;
using EventManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILoggedInUserService _loggedInUserService;

    public UserRepository(ApplicationDbContext context, ILoggedInUserService loggedInUserService)
    {
        _context = context
            ?? throw new ArgumentNullException(nameof(context));
        _loggedInUserService = loggedInUserService
            ?? throw new ArgumentNullException(nameof(loggedInUserService));
    }

    public async Task<IEnumerable<Event>> FetchUserEvents(Guid userId)
    {
        var currentUserName = _loggedInUserService.UserId;

        var joinedEvents = _context.Participants
            .Where(p => p.UserId == userId)
            .Select(p => p.Event);

        var createdEvents = _context.Events
            .Where(e => e.CreatedBy == currentUserName);

        return await joinedEvents
            .Union(createdEvents)
            .ToListAsync();
    }
}
