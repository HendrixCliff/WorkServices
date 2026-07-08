using System.Net;

namespace WorkServices.API.Tests;

public class QuoteTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public QuoteTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task QuoteNotFound_Returns404()
    {
        var response =
            await _client.GetAsync(
                $"/api/quotes/request/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}