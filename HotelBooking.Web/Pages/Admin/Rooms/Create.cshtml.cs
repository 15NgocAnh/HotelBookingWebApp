using HotelBooking.Data.Models;
using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace HotelBooking.Web.Pages.Admin.Rooms
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class CreateModel : AbstractPageModel
    {
        public CreateModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public RoomDetailsDTO Input { get; set; } = new();

        public List<RoomTypeModel> RoomTypes { get; private set; } = new();

        public async Task OnGetAsync()
        {
            await LoadRoomTypesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadRoomTypesAsync();
            if (!ModelState.IsValid) return Page();

            var response = await _httpClient.PostAsJsonAsync("api/v1/room", Input);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Failed to add room");
            return Page();
        }

        private async Task LoadRoomTypesAsync()
        {
            RoomTypes = await GetAsync<List<RoomTypeModel>>("api/v1/room/roomtypes") ?? new();
        }
    }
}
