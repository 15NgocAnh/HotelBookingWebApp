using JsonSerializer = System.Text.Json.JsonSerializer;
using ValidationException = FluentValidation.ValidationException;

namespace HotelBooking.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { errors = validationException.Errors });
                break;
            case NotFoundException _:
                code = HttpStatusCode.NotFound;
                break;
            case ForbiddenAccessException _:
                code = HttpStatusCode.Forbidden;
                break;
            case UnauthorizedAccessException _:
                code = HttpStatusCode.Unauthorized;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (string.IsNullOrEmpty(result))
        {
            var error = new
            {
                StatusCode = (int)code,
                Message = exception.Message,
                // Only include stack trace in development
                StackTrace = _environment.IsDevelopment() ? exception.StackTrace : null
            };
            result = JsonSerializer.Serialize(error);
        }

        return context.Response.WriteAsync(result);
    }
} 