using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Web.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IApiService _apiService;

        public ChangePasswordModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu hiện tại")]
            public string? CurrentPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu mới")]
            public string? NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu mới")]
            public string? ConfirmPassword { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var password = new ChangePasswordDto
            {
                CurrentPassword = Input.CurrentPassword,
                NewPassword = Input.NewPassword,
                ConfirmPassword = Input.ConfirmPassword
            };

            var result = await _apiService.PutAsync<ChangePasswordDto>("api/user/change-password", password);
            
            if (!result.IsSuccess)
            {
                foreach (var error in result.Messages)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
                return Page();
            }

            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công.";
            return Page();
        }
    }
} 