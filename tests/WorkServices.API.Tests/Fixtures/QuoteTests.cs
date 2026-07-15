using System.Net;

namespace WorkServices.API.Tests.Fixtures;

[Collection("postgres")]
public class QuoteTests
{
    private readonly HttpClient _client;

    public QuoteTests(
        PostgreSqlFixture fixture)
    {
        var factory =
            new CustomWebApplicationFactory(fixture);

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