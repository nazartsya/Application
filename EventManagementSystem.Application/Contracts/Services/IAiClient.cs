namespace EventManagementSystem.Application.Contracts.Services;

public interface IAiClient
{
    Task<string> AskAsync(string promptJson, CancellationToken cancellation = default);
}
