namespace WorkServices.Application.Interfaces;

public interface IAiService
{
    Task<string> GenerateAsync(
        string prompt,
        CancellationToken cancellationToken = default);
}