using AutoMapper;
using EventManagementSystem.Application.Contracts.Repositories;
using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Models.Events;

namespace EventManagementSystem.Infrastructure.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository repository,
        IMapper mapper)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<EventDto>> GetMyEvents(Guid userId)
    {
        var eventsFromRepo = await _repository.FetchUserEvents(userId);

        return _mapper.Map<IEnumerable<EventDto>>(eventsFromRepo);
    }
}
