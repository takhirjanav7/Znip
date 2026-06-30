using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AsosiyProject.Application.Common.Exceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            _logger.LogError(ex, "Global xatolik: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            BadRequestException => new { error = exception.Message, status = 400 },
            NotFoundException => new { error = exception.Message, status = 404 },
            ForbiddenException => new { error = exception.Message, status = 403 },
            UnauthorizedAccessException => new { error = "Login qiling", status = 401 },
            _ => new { error = "Serverda xatolik yuz berdi", status = 500 }
        };

        context.Response.StatusCode = response.status;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}