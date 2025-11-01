using EventManagementSystem.Application.Models.Assistant;

namespace EventManagementSystem.Application.Contracts.Services;

public interface IAssistantService
{
    Task<AskResponse> ProcessQuestionAsync(Guid userId, string question);
}
