using Newtonsoft.Json;

namespace HotelBooking.API.Middleware
{
    public class ExpiredTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public ExpiredTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            if (!context.Response.HasStarted && context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    status_code = (int)HttpStatusCode.Unauthorized,
                    message = "Token is invalid or has been expired."
                }));
            }
        }
    }
}
