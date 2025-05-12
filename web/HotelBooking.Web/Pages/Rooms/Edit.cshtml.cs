using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Domain.Entities;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.Web.Pages.Rooms
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class EditModel : AbstractPageModel
    {
        public EditModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public UpdateRoomDTO Room { get; set; } = new();

        public List<SelectListItem> RoomTypes { get; set; } = new();
        public List<SelectListItem> Floors { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var room = await GetAsync<RoomDTO>($"api/v1/rooms/{id}");
            if (room == null)
            {
                return NotFound();
            }

            Room = new UpdateRoomDTO
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Description = room.Description,
                FloorId = room.FloorId,
                RoomTypeId = room.RoomTypeId,
                IsActive = room.IsActive
            };

            var roomTypes = await GetAsync<List<RoomTypeDTO>>("api/v1/roomtypes");
            RoomTypes = roomTypes?.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name,
                Selected = rt.Id == room.RoomTypeId
            }).ToList() ?? new List<SelectListItem>();

            var floors = await GetAsync<List<RoomTypeDTO>>("api/v1/floors");
            Floors = floors?.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name,
                Selected = rt.Id == room.FloorId
            }).ToList() ?? new List<SelectListItem>();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var roomTypes = await GetAsync<List<RoomTypeDTO>>("api/v1/roomtypes");
                RoomTypes = roomTypes?.Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.Name,
                    Selected = rt.Id == Room.RoomTypeId
                }).ToList() ?? new List<SelectListItem>();

                return Page();
            }

            try
            {
                var response = await PutAsync<UpdateRoomDTO, RoomDTO>($"api/v1/rooms/{Room.Id}", Room);
                
                if (response != null)
                {
                    TempData["SuccessMessage"] = "Cập nhật phòng thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Cập nhật phòng thất bại");
                    var roomTypes = await GetAsync<List<RoomTypeDTO>>("api/v1/roomtypes");
                    RoomTypes = roomTypes?.Select(rt => new SelectListItem
                    {
                        Value = rt.Id.ToString(),
                        Text = rt.Name,
                        Selected = rt.Id == Room.RoomTypeId
                    }).ToList() ?? new List<SelectListItem>();

                    var floors = await GetAsync<List<RoomTypeDTO>>("api/v1/floors");
                    Floors = floors?.Select(rt => new SelectListItem
                    {
                        Value = rt.Id.ToString(),
                        Text = rt.Name,
                        Selected = rt.Id == Room.FloorId
                    }).ToList() ?? new List<SelectListItem>();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra khi cập nhật phòng: {ex.Message}");
                var roomTypes = await GetAsync<List<RoomTypeDTO>>("api/v1/roomtypes");
                RoomTypes = roomTypes?.Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.Name,
                    Selected = rt.Id == Room.RoomTypeId
                }).ToList() ?? new List<SelectListItem>();

                var floors = await GetAsync<List<RoomTypeDTO>>("api/v1/floors");
                Floors = floors?.Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.Name,
                    Selected = rt.Id == Room.FloorId
                }).ToList() ?? new List<SelectListItem>();

                return Page();
            }
        }
    }
} 