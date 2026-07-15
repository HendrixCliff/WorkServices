using System.Net.Http.Json;
using System.Text.Json;
using WorkServices.Application.Interfaces;

namespace WorkServices.Infrastructure.AI;

public class GeminiService : IAiService
{
    private readonly HttpClient _httpClient;

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GenerateAsync(
        string prompt,
        CancellationToken cancellationToken = default)
    {
        var apiKey =
            Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            ?? throw new InvalidOperationException(
                "GEMINI_API_KEY not configured.");

        var request = new
{
    contents = new[]
    {
        new
        {
            parts = new[]
            {
                new
                {
                    text = prompt
                }
            }
        }
    }
};

        var response =
            await _httpClient.PostAsJsonAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}",
                request,
                cancellationToken);

        response.EnsureSuccessStatusCode();

        using var stream =
            await response.Content.ReadAsStreamAsync(cancellationToken);

        using var document =
            await JsonDocument.ParseAsync(
                stream,
                cancellationToken: cancellationToken);

        return document
            .RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString()
            ?? "";
    }
}