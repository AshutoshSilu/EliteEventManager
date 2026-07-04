using System.Net;
using System.Text.Json;
using EliteEvents.Application.Common;
using FluentValidation;

namespace EliteEvents.API.Middleware;

/// <summary>
/// Global exception handling middleware for consistent error responses.
/// </summary>
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ApiResponse();
        int statusCode;

        switch (exception)
        {
            case ValidationException validationEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = "Validation failed.";
                response.Errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList();
                break;

            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                response.Success = false;
                response.Message = "You are not authorized to perform this action.";
                break;

            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                response.Success = false;
                response.Message = "The requested resource was not found.";
                break;

            case ArgumentException argEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                response.Success = false;
                response.Message = argEx.Message;
                break;

            case InvalidOperationException invEx:
                statusCode = (int)HttpStatusCode.Conflict;
                response.Success = false;
                response.Message = invEx.Message;
                break;

            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                response.Success = false;
                response.Message = "An unexpected error occurred. Please try again later.";
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                break;
        }

        context.Response.StatusCode = statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}
