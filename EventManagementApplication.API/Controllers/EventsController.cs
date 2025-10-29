using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Helpers;
using EventManagementSystem.Application.Models.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApplication.API.Controllers;

[Route("api/events")]
[Authorize]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;

    public EventsController(IEventService service)
    {
        _service = service
            ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents([FromQuery] string[]? tags)
    {
        var userId = UserHelper.GetUserId(User);

        return Ok(await _service.GetEventsAsync(userId, tags));
    }

    [HttpGet("{eventId}", Name = "GetEvent")]
    public async Task<ActionResult<EventDetailsDto>> GetEvent(Guid eventId)
    {
        var userId = UserHelper.GetUserId(User);

        return Ok(await _service.GetEventAsync(eventId, userId));
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent(EventForCreationDto dto)
    {
        var eventToReturn = await _service.CreateEventAsync(dto);

        return CreatedAtRoute("GetEvent",
            new { eventId = eventToReturn.Id },
            eventToReturn);
    }

    [HttpPatch("{eventId}")]
    public async Task<IActionResult> PartiallyUpdateEvent(
        Guid eventId,
        JsonPatchDocument<EventForUpdateDto> patchDocument)
    {
        await _service.PartiallyUpdateEventAsync(eventId, patchDocument);

        return NoContent();
    }

    [HttpDelete("{eventId}")]
    public async Task<ActionResult> DeleteEvent(Guid eventId)
    {
        await _service.DeleteEventAsync(eventId);

        return NoContent();
    }

    [HttpPost("{eventId}/join")]
    public async Task<IActionResult> JoinEvent(Guid eventId)
    {
        var userId = UserHelper.GetUserId(User);

        await _service.JoinEventAsync(eventId, userId);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPost("{eventId}/leave")]
    public async Task<IActionResult> LeaveEvent(Guid eventId)
    {
        var userId = UserHelper.GetUserId(User);

        await _service.LeaveEventAsync(eventId, userId);

        return StatusCode(StatusCodes.Status201Created);
    }
}
