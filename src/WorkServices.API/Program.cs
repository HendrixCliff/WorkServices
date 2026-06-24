using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using WorkServices.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using WorkServices.Infrastructure.Configurations;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Infrastructure.Authentication;
using WorkServices.Application.Interfaces;
using WorkServices.Infrastructure.UnitOfWork;

Env.Load();

var builder = WebApplication.CreateBuilder(args);


var dbHost =
    Environment.GetEnvironmentVariable("DB_HOST");

var dbPort =
    Environment.GetEnvironmentVariable("DB_PORT");

var dbName =
    Environment.GetEnvironmentVariable("DB_DATABASE");

var dbUser =
    Environment.GetEnvironmentVariable("DB_USERNAME")
   ;

var dbPassword =
    Environment.GetEnvironmentVariable("DB_PASSWORD");

    var connectionString =
    $"Host={dbHost};" +
    $"Port={dbPort};" +
    $"Database={dbName};" +
    $"Username={dbUser};" +
    $"Password={dbPassword};";

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
     options.UseNpgsql(
        connectionString,
        x =>
        {
            x.MigrationsAssembly(
                "WorkServices.Infrastructure");
        });
});

var jwtKey =
    Environment.GetEnvironmentVariable("JWT_KEY");

var jwtIssuer =
    Environment.GetEnvironmentVariable("JWT_ISSUER");

var jwtAudience =
    Environment.GetEnvironmentVariable("JWT_AUDIENCE");

   builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey!))
            };

        options.Events =
            new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken =
                        context.Request.Query["access_token"];

                    var path =
                        context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken)
                        && path.StartsWithSegments("/hubs/notifications"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
    });

builder.Services.Configure<SmtpSettings>(options =>
{
    options.Host =
        Environment.GetEnvironmentVariable("SMTP_HOST")!;

    options.Port =
        int.Parse(
            Environment.GetEnvironmentVariable("SMTP_PORT")!);

    options.Username =
        Environment.GetEnvironmentVariable("SMTP_USERNAME")!;

    options.Password =
        Environment.GetEnvironmentVariable("SMTP_PASSWORD")!;

    options.From =
        Environment.GetEnvironmentVariable("SMTP_FROM")!;
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        Environment.GetEnvironmentVariable("REDIS_CONNECTION")
        ?? "localhost:6379";
});

builder.Host.UseSerilog((context, config) =>
{
    config
        .WriteTo.Console()
        .WriteTo.Seq(
            Environment.GetEnvironmentVariable("SEQ_URL")
            ?? "http://localhost:5341");
});
//_logger.LogInformation(...)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.Run();

