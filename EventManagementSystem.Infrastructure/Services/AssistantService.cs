using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Models.Assistant;
using System.Text.Json;

namespace EventManagementSystem.Infrastructure.Services;

public class AssistantService : IAssistantService
{
    private readonly IEventService _events;
    private readonly ITagService _tags;
    private readonly IAiClient _ai;

    public AssistantService(IEventService events, ITagService tags, IAiClient ai)
    {
        _events = events
            ?? throw new ArgumentNullException(nameof(events)); ;
        _tags = tags
            ?? throw new ArgumentNullException(nameof(tags)); ;
        _ai = ai
            ?? throw new ArgumentNullException(nameof(ai)); ;
    }

    public async Task<AskResponse> ProcessQuestionAsync(Guid userId, string question)
    {
        var userEvents = await _events.GetEventsAsync(userId);
        var tags = await _tags.GetTagsAsync();

        var snapshot = new
        {
            question,
            nowUtc = DateTime.UtcNow,
            userId,
            userEvents,
            tags
        };

        var promptJson = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { WriteIndented = true });

        var answer = await _ai.AskAsync(promptJson);

        return new AskResponse { Answer = answer.Trim() };
    }
}
