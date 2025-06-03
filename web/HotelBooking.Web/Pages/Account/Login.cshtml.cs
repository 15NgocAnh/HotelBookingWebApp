using HotelBooking.Application.CQRS.Auth.Commands.Login;
using HotelBooking.Application.CQRS.Auth.DTOs;
using HotelBooking.Domain.AggregateModels.HotelAggregate;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace HotelBooking.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IApiService apiService, ILogger<LoginModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [BindProperty]
        public LoginCommand LoginInput { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _apiService.PostAsync<LoginResponseDto>("api/auth/login", LoginInput);
                if (result != null && result.Data != null)
                {
                    var userData = result.Data;

                    // Set JWT token in cookie
                    Response.Cookies.Append("JWT", userData.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });

                    // Set refresh token in cookie
                    Response.Cookies.Append("RefreshToken", userData.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    // Create claims identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userData.Id.ToString()),
                        new Claim(ClaimTypes.Name, userData.Email),
                        new Claim("FirstName", userData.FirstName), // Họ
                        new Claim("LastName", userData.LastName), // Tên
                        new Claim(ClaimTypes.Role, userData.Role),
                        new Claim("LocationAccess", "true")
                    };

                    foreach (var hotelId in userData.HotelIds)
                    {
                        claims.Add(new Claim("HotelId", hotelId.ToString()));
                    }


                    var identity = new ClaimsIdentity(claims, "Bearer");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);

                    return RedirectToPage("/Index");
                }
                else if (result != null && result.Messages != null && result.Messages.Count() > 0)
                {
                    foreach (var message in result.Messages)
                    {
                        ModelState.AddModelError(message.Field ?? string.Empty, message.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                return Page();
            }
        }
    }
}