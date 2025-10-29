using AutoMapper;
using EventManagementSystem.Application.Models.Tags;
using EventManagementSystem.Domain.Entities;

namespace EventManagementSystem.Application.Profiles;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagDto>();
        CreateMap<TagForCreationDto, Tag>();
        CreateMap<TagForUpdateDto, Tag>().ReverseMap();
    }
}
