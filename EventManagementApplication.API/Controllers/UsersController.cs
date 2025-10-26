using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Helpers;
using EventManagementSystem.Application.Models.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApplication.API.Controllers;

[Route("api/users")]
[Authorize]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService
            ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpGet("me/events")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetMyEvents()
    {
        var userId = UserHelper.GetUserId(User);

        return Ok(await _userService.GetMyEvents(userId));
    }
}
