using System.Net;
using System.Net.Http.Json;
using WorkServices.API.Tests;
using FluentAssertions;
using Xunit;

namespace WorkServices.API.Tests.Fixtures;

[Collection("postgres")]
public class AuthenticationTests
{
    private readonly HttpClient _client;

    public AuthenticationTests(
    PostgreSqlFixture fixture)
{
    var factory =
        new CustomWebApplicationFactory(fixture);

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