using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.RoomTypes
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class IndexModel : AbstractPageModel
    {
        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<RoomTypeDTO> RoomTypes { get; private set; } = new();

        public async Task OnGetAsync()
        {
            var apiResponse = await GetAsync<List<RoomTypeDTO>>("api/v1/roomtypes");
            RoomTypes = apiResponse ?? new List<RoomTypeDTO>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string roomTypeId)
        {
            try
            {
                var response = await DeleteAsync<HttpResponseMessage>($"api/v1/roomtypes/{roomTypeId}") ?? new HttpResponseMessage();
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa loại phòng thành công!";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Xóa loại phòng thất bại: {errorMessage}";
                }
                return Redirect("/RoomTypes");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra khi xóa loại phòng: {ex.Message}";
                return Redirect("/RoomTypes");
            }
        }
    }
} 