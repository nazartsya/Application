using AutoMapper;
using EventManagementSystem.Application.Contracts;
using EventManagementSystem.Application.Contracts.Repositories;
using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Exceptions;
using EventManagementSystem.Application.Helpers;
using EventManagementSystem.Application.Models.Events;
using EventManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventManagementSystem.Infrastructure.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggedInUserService _loggedInUserService;

    public EventService(
        IEventRepository repository,
        IMapper mapper,
        IServiceProvider serviceProvider,
        ILoggedInUserService loggedInUserService)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _serviceProvider = serviceProvider
            ?? throw new ArgumentNullException(nameof(serviceProvider));
        _loggedInUserService = loggedInUserService
            ?? throw new ArgumentNullException(nameof(loggedInUserService));
    }

    public async Task<IEnumerable<EventDto>> GetEventsAsync(Guid? userId, IEnumerable<string>? tagNames = null)
    {
        var events = await _repository.GetEventsAsync(tagNames);

        var result = _mapper.Map<IEnumerable<EventDto>>(events);

        foreach (var dto in result)
        {
            dto.IsJoined = userId != null && events
                .First(e => e.Id == dto.Id)
                .Participants.Any(p => p.UserId == userId);
        }

        return result;
    }

    public async Task<EventDetailsDto> GetEventAsync(Guid eventId, Guid userId)
    {
        var entity = await _repository.GetEventAsync(eventId)
            ?? throw new NotFoundException($"Event with id {eventId} not found.");

        var dto = _mapper.Map<EventDetailsDto>(entity);

        dto.IsJoined = entity.Participants.Any(p => p.UserId == userId);

        return dto;
    }

    public async Task<EventDto> CreateEventAsync(EventForCreationDto dto)
    {
        await ValidationHelper.ThrowIfInvalidAsync(dto, _serviceProvider);

        var entity = _mapper.Map<Event>(dto);
        await _repository.AddEvent(entity);
        await _repository.SaveAsync();

        return _mapper.Map<EventDto>(entity);
    }

    public async Task PartiallyUpdateEventAsync(Guid eventId, JsonPatchDocument<EventForUpdateDto> patchDoc)
    {
        var entity = await _repository.GetEventAsync(eventId)
            ?? throw new NotFoundException($"Event with id {eventId} not found.");

        if (entity.CreatedBy != _loggedInUserService.UserId)
            throw new ForbiddenException("You are not allowed to edit this event.");

        var dto = _mapper.Map<EventForUpdateDto>(entity);
        var modelState = new ModelStateDictionary();
        patchDoc.ApplyTo(dto, modelState);

        if (!modelState.IsValid)
        {
            var errors = modelState
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage)
                .ToArray());

            throw new CustomValidationException(errors);
        }

        await ValidationHelper.ThrowIfInvalidAsync(dto, _serviceProvider);

        _mapper.Map(dto, entity);
        await _repository.UpdateEvent(entity);
        await _repository.SaveAsync();
    }

    public async Task DeleteEventAsync(Guid eventId)
    {
        var entity = await _repository.GetEventAsync(eventId)
            ?? throw new NotFoundException($"Event with id {eventId} not found.");

        if (entity.CreatedBy != _loggedInUserService.UserId)
            throw new ForbiddenException("You are not allowed to delete this event.");

        _repository.DeleteEvent(entity);
        await _repository.SaveAsync();
    }

    public async Task JoinEventAsync(Guid eventId, Guid userId)
    {
        var @event = await _repository.GetEventWithParticipantsAsync(eventId)
            ?? throw new NotFoundException("Event not found.");

        var alreadyJoined = @event.Participants.Any(p => p.UserId == userId);

        if (alreadyJoined)
        {
            throw new BadRequestException("User already joined this event.");
        }

        if (@event.Capacity.HasValue && @event.Participants.Count >= @event.Capacity.Value)
        {
            throw new BadRequestException("This event has reached its participant limit.");
        }

        var participant = new Participant { EventId = eventId, UserId = userId };
        _repository.JoinEvent(participant);
        await _repository.SaveAsync();
    }

    public async Task LeaveEventAsync(Guid eventId, Guid userId)
    {
        var participant = await _repository.GetParticipantAsync(eventId, userId)
            ?? throw new NotFoundException("Participant not found in this event.");
        _repository.LeaveEvent(participant);
        await _repository.SaveAsync();
    }
}
