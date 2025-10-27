using EventManagementSystem.Domain.Entities;

namespace EventManagementSystem.Application.Contracts.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<Event>> FetchUserEvents(Guid userId);
}
