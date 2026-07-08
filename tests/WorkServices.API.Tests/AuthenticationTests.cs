using System.Net;
using System.Net.Http.Json;
using WorkServices.API.Tests;
using FluentAssertions;
using Xunit;

public class AuthenticationTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthenticationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

 [Fact]
public async Task Login_ReturnsSuccess()
{
    var response = await _client.PostAsJsonAsync(
        "/api/auth/login",
        new
        {
            Email = "admin@test.com",
            Password = "Password123!"
        });

    var body = await response.Content.ReadAsStringAsync();

    Console.WriteLine(body);

    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
}