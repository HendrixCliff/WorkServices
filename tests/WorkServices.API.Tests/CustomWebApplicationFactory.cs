using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkServices.API.Tests.Fixtures;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;
using WorkServices.Domain.Enums;
using WorkServices.Infrastructure.Persistence;

namespace WorkServices.API.Tests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly PostgreSqlFixture _fixture;

    public CustomWebApplicationFactory(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

       builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JWT_KEY"] =
                    "ThisIsAVeryLongDevelopmentTestingSecretKey123456789",

                ["JWT_ISSUER"] = "WorkServices",
                ["JWT_AUDIENCE"] = "WorkServices"
            });
        });

        builder.ConfigureServices(services =>
{
    var descriptor = services.SingleOrDefault(
        d => d.ServiceType ==
             typeof(DbContextOptions<ApplicationDbContext>));

    if (descriptor != null)
    {
        services.Remove(descriptor);
    }

    services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(_fixture.ConnectionString);
    });

    var provider = services.BuildServiceProvider();

    using var scope = provider.CreateScope();

    var db = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    db.Database.Migrate();

    if (!db.Users.Any())
    {
        var hasher = scope.ServiceProvider
            .GetRequiredService<IPasswordHasher>();

        var customer = new Customer(
            fullName: "Admin User",
            email: "admin@test.com",
            phoneNumber: "08012345678",
            passwordHash: hasher.Hash("Password123!")
        );

        db.Users.Add(customer);

        db.SaveChanges();
    }
});
    }
}