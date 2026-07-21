using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkServices.Infrastructure.Persistence;

public sealed class ApplicationDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var apiPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "../WorkServices.API");

        // Load the same .env file used by the API
        Env.Load(Path.Combine(apiPath, ".env"));

        var connectionString =
            $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
            $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
            $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
            $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
            $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>();

        options.UseNpgsql(connectionString);

        return new ApplicationDbContext(options.Options);
    }
}