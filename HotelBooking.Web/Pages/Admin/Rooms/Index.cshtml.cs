using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Admin.Rooms
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class IndexModel : AbstractPageModel
    {
        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<RoomDTO> Rooms { get; private set; } = new();

        public async Task OnGetAsync()
        {
            Rooms = await GetAsync<List<RoomDTO>>("api/v1/room") ?? new List<RoomDTO>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string roomId)
        {
            var response = await DeleteRoomAsync(roomId);
            return response.IsSuccessStatusCode ? RedirectToPage("Index") : HandleDeleteError();
        }

        private async Task<HttpResponseMessage> DeleteRoomAsync(string roomId)
        {
            return await DeleteAsync<HttpResponseMessage>($"api/v1/room/{roomId}") ?? new HttpResponseMessage();
        }

        private IActionResult HandleDeleteError()
        {
            ModelState.AddModelError(string.Empty, "Failed to delete room.");
            return Page();
        }
    }
}
