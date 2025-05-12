namespace HotelBooking.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TokenExpiredHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenExpiredHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            // Kiểm tra xem token có hết hạn không
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                // Token hết hạn, trả về lỗi 401 Unauthorized và thông báo cho người dùng
                httpContext.Response.StatusCode = 401;
                httpContext.Response.WriteAsync("Token has expired. Please log in again.");
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TokenExpiredHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenExpiredHandleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenExpiredHandleMiddleware>();
        }
    }
}
