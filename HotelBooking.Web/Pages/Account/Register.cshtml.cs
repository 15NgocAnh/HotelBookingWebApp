using HotelBooking.Domain.DTOs.User;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HotelBooking.Web.Pages.Account
{
    public class RegisterModel : AbstractPageModel
    {
        public RegisterModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public UserRegisterDTO Input { get; set; } = new();

        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/register", Input);

            if (response.IsSuccessStatusCode)
            {
                IsSuccess = true;
                return Page();
            }
            try
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorObj = JsonSerializer.Deserialize<ApiErrorResponse>(errorContent);
                ErrorMessage = errorObj?.Message ?? "Đăng ký thất bại! Vui lòng thử lại.";
            }
            catch
            {
                ErrorMessage = "Có lỗi xảy ra! Vui lòng thử lại.";
            }

            return Page();
        }
    }

    public class ApiErrorResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
