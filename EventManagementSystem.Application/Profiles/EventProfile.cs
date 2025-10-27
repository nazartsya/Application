using AutoMapper;
using EventManagementSystem.Application.Models.Events;
using EventManagementSystem.Domain.Entities;

namespace EventManagementSystem.Application.Profiles;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.ParticipantsCount, opt => opt.MapFrom(src => src.Participants.Count))
            .ForMember(dest => dest.IsJoined, opt => opt.Ignore());

        CreateMap<Event, EventDetailsDto>()
            .IncludeBase<Event, EventDto>()
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));

        CreateMap<EventForCreationDto, Event>();
        CreateMap<EventForUpdateDto, Event>().ReverseMap();
    }
}
