using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Helpers;
using EventManagementSystem.Application.Models.Assistant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApplication.API.Controllers;

[Route("api/assistant")]
[Authorize]
[ApiController]
public class AssistantController : ControllerBase
{
    private readonly IAssistantService _assistant;

    public AssistantController(IAssistantService assistant)
    {
        _assistant = assistant
            ?? throw new ArgumentNullException(nameof(assistant));
    }

    [HttpPost("ask")]
    public async Task<ActionResult<AskResponse>> Ask(AskRequest dto)
    {
        var userId = UserHelper.GetUserId(User);
        return Ok(await _assistant.ProcessQuestionAsync(userId, dto.Question));
    }
}
