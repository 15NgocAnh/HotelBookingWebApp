using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Floors
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class IndexModel : AbstractPageModel
    {
        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<FloorDTO> Floors { get; private set; } = new();

        public async Task OnGetAsync()
        {
            var apiResponse = await GetAsync<List<FloorDTO>>("api/v1/floors");
            Floors = apiResponse ?? new List<FloorDTO>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string floorId)
        {
            try
            {
                var response = await DeleteAsync<HttpResponseMessage>($"api/v1/floors/{floorId}") ?? new HttpResponseMessage();
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa tầng thành công!";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Xóa tầng thất bại: {errorMessage}";
                }
                return Redirect("/Floors");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra khi xóa tầng: {ex.Message}";
                return Redirect("/Floors");
            }
        }
    }
} 