using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using WorkServices.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Infrastructure.Configurations;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Infrastructure.Authentication;
using WorkServices.Application.Interfaces;
using WorkServices.Application;
using WorkServices.API.Hubs;
using WorkServices.API.Middleware;
using WorkServices.Infrastructure.UnitOfWork;
using WorkServices.Infrastructure.DomainEvents;
using WorkServices.Infrastructure.Services;
using WorkServices.Infrastructure.Persistence.Repositories;
using Microsoft.OpenApi.Models;
using WorkServices.API.Services;
using System.Threading.RateLimiting;

Env.Load();


var builder = WebApplication.CreateBuilder(args);
Console.WriteLine(typeof(Program).Assembly.FullName);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    var connectionString =
        $"Host={configuration["DB_HOST"]};" +
        $"Port={configuration["DB_PORT"]};" +
        $"Database={configuration["DB_DATABASE"]};" +
        $"Username={configuration["DB_USERNAME"]};" +
        $"Password={configuration["DB_PASSWORD"]};";

    Console.WriteLine($"Using DB = {connectionString}");

    options.UseNpgsql(connectionString,
        npgsql =>
        {
            npgsql.MigrationsAssembly("WorkServices.Infrastructure");
        });
});
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
        var configuration = builder.Configuration;

        var jwtKey = configuration["JWT_KEY"];
        var jwtIssuer = configuration["JWT_ISSUER"];
        var jwtAudience = configuration["JWT_AUDIENCE"];

        Console.WriteLine($"Inside AddJwtBearer");
        Console.WriteLine($"JWT_KEY = {jwtKey}");
        Console.WriteLine($"JWT_ISSUER = {jwtIssuer}");
        Console.WriteLine($"JWT_AUDIENCE = {jwtAudience}");

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "JWT_KEY is missing from configuration.");
        }

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
                        Encoding.UTF8.GetBytes(jwtKey))
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

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/hubs/notifications"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
    });
Console.WriteLine("Program started");
Console.WriteLine(builder.Environment.EnvironmentName);

foreach (var pair in builder.Configuration.AsEnumerable())
{
    if (pair.Key.Contains("JWT"))
        Console.WriteLine($"{pair.Key} = {pair.Value}");
}
Console.WriteLine("JWT_KEY before GetBytes = " +
    builder.Configuration["JWT_KEY"]);

Console.WriteLine("Issuer = " +
    builder.Configuration["JWT_ISSUER"]);

Console.WriteLine("Audience = " +
    builder.Configuration["JWT_AUDIENCE"]);
 

builder.Services.Configure<SmtpSettings>(options =>
{
   options.Host =
    builder.Configuration["SMTP_HOST"]!;

options.Port =
    int.Parse(builder.Configuration["SMTP_PORT"]!);

options.Username =
    builder.Configuration["SMTP_USERNAME"]!;

options.Password =
    builder.Configuration["SMTP_PASSWORD"]!;

options.From =
    builder.Configuration["SMTP_FROM"]!;
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
    builder.Configuration["REDIS_CONNECTION"]
    ?? "localhost:6379";
});

builder.Host.UseSerilog((context, config) =>
{
    config
        .WriteTo.Console()
       .WriteTo.Seq(
    builder.Configuration["SEQ_URL"]
    ?? "http://localhost:5341");
});
//_logger.LogInformation(...)
builder.Services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IJobAssignmentRepository, JobAssignmentRepository>();
builder.Services.AddScoped< IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
builder.Services.AddScoped< IArtisanRepository, ArtisanRepository>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRealtimeNotifier, RealtimeNotifier>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter =
        PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "global",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 10
                }));
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "CustomerOnly",
        p => p.RequireRole("Customer"));

    options.AddPolicy(
        "ArtisanOnly",
        p => p.RequireRole("Artisan"));

    options.AddPolicy(
        "AdminOnly",
        p => p.RequireRole("Admin"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Work Services API",
            Version = "v1",
            Description =
                "Work Services Platform API"
        });

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Enter JWT like: Bearer {token}"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

builder.Services.AddSignalR();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Work Services API v1");

        options.RoutePrefix = string.Empty;
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();

public partial class Program
{
}

