using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HotelBooking.Web.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly IApiService _apiService;

        public ProfileModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Vui lòng nhập tên")]
            [Display(Name = "Tên")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập họ")]
            [Display(Name = "Họ")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập email")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _apiService.GetAsync<UserDto>($"api/user/{userId}");
            
            if (result == null || !result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Không thể tải thông tin người dùng.");
                return Page();
            }

            Input = new InputModel()
            {
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                Email = result.Data.Email,
                PhoneNumber = result.Data.Phone
            };
            TempData["SuccessMessage"] = "Update user profile successfully!";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var profile = new UpdateProfileDto
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                PhoneNumber = Input.PhoneNumber
            };

            var result = await _apiService.PutAsync<UpdateProfileDto>("api/user/profile", profile);
            
            if (!result.IsSuccess)
            {
                foreach (var error in result.Messages)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
                return Page();
            }

            TempData["StatusMessage"] = "Thông tin cá nhân đã được cập nhật thành công.";
            return RedirectToPage();
        }
    }
} 