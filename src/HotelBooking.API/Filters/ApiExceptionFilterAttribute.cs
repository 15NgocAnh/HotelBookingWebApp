using Microsoft.AspNetCore.Mvc.Filters;
using ValidationException = HotelBooking.Application.Common.Exceptions.ValidationException;

namespace HotelBooking.API.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        _logger = logger;
        // Register known exception types and handlers
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.TryGetValue(type, out var handler))
        {
            handler.Invoke(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var errors = exception.Errors
            .SelectMany(kvp => kvp.Value.Select(msg => new
            {
                field = kvp.Key,
                message = msg
            }))
            .ToList();

        var result = new
        {
            status = StatusCodes.Status400BadRequest,
            messages = errors
        };

        context.Result = new BadRequestObjectResult(result);
        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var result = new
        {
            status = StatusCodes.Status404NotFound,
            messages = new[]
            {
                new { field = "", message = exception.Message }
            }
        };

        context.Result = new NotFoundObjectResult(result);
        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var result = new
        {
            status = (int)HttpStatusCode.Unauthorized,
            messages = new[]
            {
                new { field = "", message = "Unauthorized access." }
            }
        };

        context.Result = new ObjectResult(result)
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        };

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        var result = new
        {
            status = (int)HttpStatusCode.Forbidden,
            messages = new[]
            {
                new { field = "", message = "You do not have permission to access this resource." }
            }
        };

        context.Result = new ObjectResult(result)
        {
            StatusCode = (int)HttpStatusCode.Forbidden
        };

        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An unhandled exception occurred");

        var result = new
        {
            status = (int)HttpStatusCode.InternalServerError,
            messages = new[]
            {
                new { field = "", message = "An unexpected error occurred. Please try again later." }
            }
        };

        context.Result = new ObjectResult(result)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        context.ExceptionHandled = true;
    }
} 