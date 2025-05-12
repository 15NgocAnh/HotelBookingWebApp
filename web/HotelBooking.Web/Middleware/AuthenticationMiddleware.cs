using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace HotelBooking.Web.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Check if user is authenticated
                var isAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;
                
                // Get the current path
                var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
                
                // Skip authentication check for login page and static files
                if (path.StartsWith("/account/login") || 
                    path.StartsWith("/css") || 
                    path.StartsWith("/js") || 
                    path.StartsWith("/lib") ||
                    path.StartsWith("/images"))
                {
                    await _next(context);
                    return;
                }

                // Check JWT token
                var token = context.Request.Cookies["JWT"];
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No JWT token found, redirecting to login");
                    await RedirectToLogin(context);
                    return;
                }

                // Validate JWT token
                var principal = context.User;
                if (principal == null || !principal.Claims.Any())
                {
                    _logger.LogWarning("Invalid or expired JWT token, redirecting to login");
                    await RedirectToLogin(context);
                    return;
                }

                // Check location access
                var locationClaim = principal.FindFirst("LocationAccess");
                if (locationClaim == null)
                {
                    _logger.LogWarning("No location access permission, redirecting to login");
                    await RedirectToLogin(context);
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AuthenticationMiddleware");
                await RedirectToLogin(context);
            }
        }

        private async Task RedirectToLogin(HttpContext context)
        {
            // Clear any existing authentication
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            // Clear JWT cookie
            context.Response.Cookies.Delete("JWT");
            
            // Redirect to login page with return URL
            var returnUrl = context.Request.Path + context.Request.QueryString;
            var loginUrl = $"/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}";
            context.Response.Redirect(loginUrl);
        }
    }
} 