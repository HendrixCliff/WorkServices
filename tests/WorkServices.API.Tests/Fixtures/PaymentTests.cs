using System.Net;

namespace WorkServices.API.Tests.Fixtures;

[Collection("postgres")]
public class PaymentTests
{
    private readonly HttpClient _client;

    public PaymentTests(
    PostgreSqlFixture fixture)
{
    var factory =
        new CustomWebApplicationFactory(fixture);

    _client = factory.CreateClient();
}

    [Fact]
    public async Task UnknownPayment_Returns404()
    {
        var response =
            await _client.GetAsync(
                $"/api/payments/request/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}