using System.Net;
using System.Text.Json;
using WorkServices.Application.Common.Exceptions;

namespace WorkServices.API.Middleware;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

  public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, ex.Message);

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = ex switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new
            {
                message = ex.Message
            }));
    }
}
}