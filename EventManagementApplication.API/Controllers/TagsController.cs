using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Models.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApplication.API.Controllers;

[Route("api/tags")]
[Authorize]
[ApiController]
public class TagsController : ControllerBase
{
    private readonly ITagService _service;

    public TagsController(ITagService service)
    {
        _service = service
            ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
    {
        return Ok(await _service.GetTagsAsync());
    }

    [HttpGet("{tagId}", Name = "GetTag")]
    public async Task<ActionResult<TagDto>> GetTag(Guid tagId)
    {
        return Ok(await _service.GetTagAsync(tagId));
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> CreateTag(TagForCreationDto dto)
    {
        var tagToReturn = await _service.CreateTagAsync(dto);

        return CreatedAtRoute("GetTag",
            new { tagId = tagToReturn.Id },
            tagToReturn);
    }

    [HttpPatch("{tagId}")]
    public async Task<IActionResult> PartiallyUpdateTag(
        Guid tagId,
        JsonPatchDocument<TagForUpdateDto> patchDocument)
    {
        await _service.PartiallyUpdateTagAsync(tagId, patchDocument);

        return NoContent();
    }

    [HttpDelete("{tagId}")]
    public async Task<ActionResult> DeleteTag(Guid tagId)
    {
        await _service.DeleteTagAsync(tagId);

        return NoContent();
    }
}
