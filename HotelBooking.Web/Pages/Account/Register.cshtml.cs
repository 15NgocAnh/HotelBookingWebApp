using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Net.Http;

namespace HotelBooking.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RegisterModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("BackendApi");
            _httpClient.BaseAddress = new Uri(_configuration["BackendApi"]);
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        [TempData]
        public string SuccessMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/register", Input);
            var responseText = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(responseText);

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToPage("./Login");
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMessage);

            return Page();
        }

        public class RegisterInputModel
        {
            [Required]
            [StringLength(50, MinimumLength = 1, ErrorMessage = "First Name must contain at least 1 character and maximum to 50 characters.")]
            [JsonPropertyName("first_name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(10, MinimumLength = 1, ErrorMessage = "Last Name must contain at least 1 character and maximum to 10 characters.")]
            [JsonPropertyName("last_name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(70, MinimumLength = 5, ErrorMessage = "Email length must be between 5 and 70 characters.")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [StringLength(11, MinimumLength = 9)]
            [JsonPropertyName("phone_number")]
            public string PhoneNumber { get; set; }

            [Required]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Gender Gender { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateOnly Dob { get; set; }

            [Required]
            public string Address { get; set; }

            [Column(TypeName = "text")]
            public string? Avatar { get; set; }
        }
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
