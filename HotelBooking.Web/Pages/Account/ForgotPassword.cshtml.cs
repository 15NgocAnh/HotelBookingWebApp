using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Account
{
    public class ForgotPasswordModel : AbstractPageModel
    {
        public ForgotPasswordModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public string Email { get; set; }

        public string? ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PostAsJsonAsync($"api/v1/auth/forgot?email={Email}", "");

            if (response.IsSuccessStatusCode)
            {
                IsSuccess = true;
            }
            else
            {
                ErrorMessage = "This email was not found.";
            }

            return Page();
        }
    }
}
