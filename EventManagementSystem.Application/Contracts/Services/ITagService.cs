using EventManagementSystem.Application.Models.Tags;
using Microsoft.AspNetCore.JsonPatch;

namespace EventManagementSystem.Application.Contracts.Services;

public interface ITagService
{
    Task<IEnumerable<TagDto>> GetTagsAsync();
    Task<TagDto> GetTagAsync(Guid tagId);
    Task<TagDto> CreateTagAsync(TagForCreationDto dto);
    Task PartiallyUpdateTagAsync(Guid tagId, JsonPatchDocument<TagForUpdateDto> patchDoc);
    Task DeleteTagAsync(Guid tagId);
}
