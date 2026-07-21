using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence;

public static class ApplicationDbSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();

    var context =
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var hasher =
        scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    await context.Database.MigrateAsync();

    var existing = await context.Users
        .FirstOrDefaultAsync(x => x.Email == "admin@test.com");

    if (existing != null)
    {
        if (!existing.EmailConfirmed)
        {
            existing.ConfirmEmail();
            await context.SaveChangesAsync();
        }

        return;
    }

    var admin = new Admin(
        "System Administrator",
        "admin@test.com",
        "08000000000",
        hasher.Hash("Password123!")
    );

    admin.ConfirmEmail();

    Console.WriteLine($"Before Save = {admin.EmailConfirmed}");

    context.Admins.Add(admin);

    await context.SaveChangesAsync();

    Console.WriteLine($"After Save = {admin.EmailConfirmed}");
}
}