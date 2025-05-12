using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.Web.Pages.Rooms
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class CreateModel : AbstractPageModel
    {
        public CreateModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public CreateRoomDTO Room { get; set; } = new();

        public List<SelectListItem> RoomTypes { get; set; } = new();
        public List<SelectListItem> Floors { get; set; } = new();

        public async Task OnGetAsync()
        {
            var roomTypes = await GetAsync<List<RoomTypeDTO>>("api/v1/roomtypes");
            RoomTypes = roomTypes?.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name
            }).ToList() ?? new List<SelectListItem>();

            var floors = await GetAsync<List<FloorDTO>>("api/v1/floors");
            Floors = floors?.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList() ?? new List<SelectListItem>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                var response = await PostAsync<CreateRoomDTO, RoomDTO>("api/v1/rooms", Room);
                
                if (response != null)
                {
                    TempData["SuccessMessage"] = "Thêm mới phòng thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Thêm mới phòng thất bại");
                    await OnGetAsync();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra khi thêm mới phòng: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }
    }
} 