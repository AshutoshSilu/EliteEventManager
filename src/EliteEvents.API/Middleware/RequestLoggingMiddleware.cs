using System.Diagnostics;

namespace EliteEvents.API.Middleware;

/// <summary>
/// Logs HTTP request details including method, path, status code, and duration.
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var statusCode = context.Response.StatusCode;
        var method = context.Request.Method;
        var path = context.Request.Path;
        var duration = stopwatch.ElapsedMilliseconds;

        if (statusCode >= 500)
        {
            _logger.LogError("{Method} {Path} responded {StatusCode} in {Duration}ms", method, path, statusCode, duration);
        }
        else if (statusCode >= 400)
        {
            _logger.LogWarning("{Method} {Path} responded {StatusCode} in {Duration}ms", method, path, statusCode, duration);
        }
        else
        {
            _logger.LogInformation("{Method} {Path} responded {StatusCode} in {Duration}ms", method, path, statusCode, duration);
        }
    }
}
