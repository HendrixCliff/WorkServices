using System.Net;

namespace WorkServices.API.Tests;

public class ServiceRequestTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ServiceRequestTests(
        CustomWebApplicationFactory factory)
    {
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