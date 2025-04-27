using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Rooms
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class IndexModel : AbstractPageModel
    {
        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<RoomDTO> Rooms { get; private set; } = new();

        public class BranchFloorRoomViewModel
        {
            public string BranchName { get; set; }
            public int BranchId { get; set; }
            public List<FloorGroup> Floors { get; set; }
            public class FloorGroup
            {
                public int FloorId { get; set; }
                public string FloorName { get; set; }
                public List<RoomDTO> Rooms { get; set; }
            }
        }
        public List<BranchFloorRoomViewModel> BranchFloorRooms { get; set; } = new();

        public async Task OnGetAsync()
        {
            var branches = await GetAsync<List<BranchDTO>>("api/v1/branches/all");
            var floors = await GetAsync<List<FloorDTO>>("api/v1/floors");
            var rooms = await GetAsync<List<RoomDTO>>("api/v1/rooms");
            BranchFloorRooms = branches.Select(branch => new BranchFloorRoomViewModel
            {
                BranchId = int.Parse(branch.Id),
                BranchName = branch.Name,
                Floors = floors != null ? floors.Where(f => f.BranchId == int.Parse(branch.Id)).OrderBy(f => f.OrderFloor).Select(floor => new BranchFloorRoomViewModel.FloorGroup
                {
                    FloorId = floor.Id,
                    FloorName = floor.Name,
                    Rooms = rooms != null ? rooms.Where(r => r.FloorId == floor.Id).OrderBy(r => r.RoomNumber).ToList() : new()
                }).ToList() : new()
            }).Where(b => b.Floors.Any()).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string roomId)
        {
            try
            {
                var response = await DeleteAsync<HttpResponseMessage>($"api/v1/rooms/{roomId}") ?? new HttpResponseMessage();
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa phòng thành công!";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Xóa phòng thất bại: {errorMessage}";
                }
                return Redirect("/Rooms");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra khi xóa phòng: {ex.Message}";
                return Redirect("/Rooms");
            }
        }
    }
} 