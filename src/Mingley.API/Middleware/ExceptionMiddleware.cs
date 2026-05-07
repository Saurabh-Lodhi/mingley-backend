using System.Net;
using System.Text.Json;
using Mingley.Application.DTOs.Common;

namespace Mingley.API.Middleware;

/// <summary>Centralized exception handler — returns consistent ApiResponse on all errors.</summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business logic error: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteResponse(context, HttpStatusCode.InternalServerError,
                "An unexpected error occurred. Please try again.");
        }
    }

    private static async Task WriteResponse(HttpContext ctx, HttpStatusCode code, string message)
    {
        ctx.Response.StatusCode = (int)code;
        ctx.Response.ContentType = "application/json";
        var response = ApiResponse<object>.Fail(message, (int)code);
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
