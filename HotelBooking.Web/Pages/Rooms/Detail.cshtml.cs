using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Rooms
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class DetailModel : AbstractPageModel
    {
        public DetailModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public RoomDTO Room { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Room = await GetAsync<RoomDTO>($"api/v1/rooms/{id}");
            if (Room == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
} 