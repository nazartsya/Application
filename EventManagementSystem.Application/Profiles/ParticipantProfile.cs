using AutoMapper;
using EventManagementSystem.Application.Models.Participants;
using EventManagementSystem.Domain.Entities;

namespace EventManagementSystem.Application.Profiles;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<Participant, ParticipantDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName));
    }
}
