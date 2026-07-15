using System.Net;

namespace WorkServices.API.Tests.Fixtures;

[Collection("postgres")]
public class ServiceRequestTests
{
    private readonly HttpClient _client;

    public ServiceRequestTests(
    PostgreSqlFixture fixture)
{
    var factory =
        new CustomWebApplicationFactory(fixture);

    _client = factory.CreateClient();
}

    [Fact]
    public async Task GetUnknownServiceRequest_Returns404()
    {
        var response =
            await _client.GetAsync(
                $"/api/service-requests/{Guid.NewGuid()}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}