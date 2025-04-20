using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Account
{
    public class VerifyModel : AbstractPageModel
    {
        public VerifyModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public string? ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["JWT"];
            var response = await _httpClient.GetAsync($"api/v1/auth/verify/{token}");

            if (response.IsSuccessStatusCode)
            {
                IsSuccess = true;
                ErrorMessage = "Email verification successful!";
            }
            else
            {
                ErrorMessage = "Verification failed!";
            }

            return Page();
        }
    }
}
