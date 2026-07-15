using Testcontainers.PostgreSql;

namespace WorkServices.API.Tests.Fixtures;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; }

    public PostgreSqlFixture()
    {
       Container = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("workservices")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    }

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }

    public string ConnectionString =>
        Container.GetConnectionString();
}