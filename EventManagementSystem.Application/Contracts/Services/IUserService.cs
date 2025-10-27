using EventManagementSystem.Application.Models.Events;

namespace EventManagementSystem.Application.Contracts.Services;

public interface IUserService
{
    Task<IEnumerable<EventDto>> GetMyEvents(Guid userId);
}
