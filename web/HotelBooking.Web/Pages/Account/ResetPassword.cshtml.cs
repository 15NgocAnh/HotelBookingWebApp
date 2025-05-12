using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace HotelBooking.Web.Pages.Account
{
    public class ResetPasswordModel : AbstractPageModel
    {
        public ResetPasswordModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public string? Token { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "M?t kh?u kh�ng ???c ?? tr?ng")]
        [MinLength(6, ErrorMessage = "M?t kh?u ph?i �t nh?t 6 k� t?")]
        public string NewPassword { get; set; } = string.Empty;

        [BindProperty]
        [Compare(nameof(NewPassword), ErrorMessage = "M?t kh?u x�c nh?n kh�ng kh?p")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }

        public void OnGet(string token)
        {
            Token = token;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var request = new
            {
                Token,
                NewPassword
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/v1/auth/reset-password", content);

            if (response.IsSuccessStatusCode)
            {
                IsSuccess = true;
            }
            else
            {
                ErrorMessage = "Kh�ng th? ??t l?i m?t kh?u. Vui l�ng th? l?i.";
            }

            return Page();
        }
    }
}
