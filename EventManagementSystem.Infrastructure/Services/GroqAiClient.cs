using EventManagementSystem.Application.Contracts.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EventManagementSystem.Infrastructure.Services;

public class GroqAiClient : IAiClient
{
    private readonly HttpClient _http;
    private readonly string _model;

    public GroqAiClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _model = config["Ai:Model"] ?? "llama-3.3-70b-versatile";
        var key = config["Ai:ApiKey"];
        if (!string.IsNullOrEmpty(key))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
    }

    public async Task<string> AskAsync(string promptJson, CancellationToken cancellation = default)
    {
        var system = "You are a helpful assistant for an event management system. Use ONLY the provided JSON data. " +
                     "Answer concisely. If unclear, reply: \"Sorry, I didn’t understand. Please try rephrasing your question.\"";

        var body = new
        {
            model = _model,
            messages = new[]
            {
                new { role = "system", content = system },
                new { role = "user", content = promptJson }
            },
            temperature = 0.0
        };

        var json = JsonSerializer.Serialize(body);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("chat/completions", content, cancellation);
        response.EnsureSuccessStatusCode();
        var resText = await response.Content.ReadAsStringAsync(cancellation);
        using var doc = JsonDocument.Parse(resText);

        if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
        {
            var msg = choices[0].GetProperty("message").GetProperty("content").GetString();
            return msg ?? "Sorry, I didn't understand. Please try rephrasing your question.";
        }

        return "Sorry, I didn't understand. Please try rephrasing your question.";
    }
}
