using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelBooking.Web.Pages.Abstract;
using HotelBooking.Domain.DTOs.Authentication;

namespace HotelBooking.Web.Pages.Account
{
    public class LogoutModel : AbstractPageModel
    {
        public LogoutModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy token từ Session
            var token = Request.Cookies["RefreshToken"];
            if (!string.IsNullOrEmpty(token))
            {
                await _httpClient.PostAsJsonAsync("api/v1/auth/logout", new TokenDTO { Token = token });
            }

            // Xóa toàn bộ session
            HttpContext.Session.Clear();

            // Xóa tất cả cookie liên quan đến phiên đăng nhập
            Response.Cookies.Delete("JWT");
            Response.Cookies.Delete("RefreshToken");

            // Đăng xuất khỏi hệ thống 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Chuyển hướng về trang chủ
            return RedirectToPage("/Index");
        }
    }
}
