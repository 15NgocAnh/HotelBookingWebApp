using Newtonsoft.Json;

namespace HotelBooking.API.Middleware
{
    public class ValidationExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionHandlerMiddleware> _logger;

        public ValidationExceptionHandlerMiddleware(RequestDelegate next, ILogger<ValidationExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            if (!context.Response.HasStarted && context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
            {

                var validationErrors = new Dictionary<string, string>();
                foreach (var key in context.Items.Keys)
                {
                    if (context.Items[key] is string error)
                    {
                        validationErrors[key.ToString()] = error;
                    }
                }
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    error = "Validation failed.",
                    validationErrors
                }));
            }
        }
    }

}
