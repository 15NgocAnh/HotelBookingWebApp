using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBooking.Web.Pages.Admin.Rooms
{
    public class EditModel : AbstractPageModel
    {
        public EditModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public RoomDetailsDTO Room { get; set; } = new();

        public List<RoomTypeModel> RoomTypes { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            await LoadRoomTypes();
            Room = await GetAsync<RoomDetailsDTO>($"api/v1/room/{id}") ?? new();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadRoomTypes();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return await UpdateRoom();
        }

        private async Task<IActionResult> UpdateRoom()
        {
            try
            {
                var response = await PutAsync<RoomDetailsDTO, RoomDetailsDTO>($"api/v1/room/{Room.Id}", Room);
                if (response != null)
                {
                    TempData["SuccessMessage"] = "Room updated successfully!";
                    return RedirectToPage("./Index");
                }
                ErrorMessage = "Update failed!";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error updating room: " + ex.Message;
                return Page();
            }
        }

        private async Task LoadRoomTypes()
        {
            RoomTypes = await GetAsync<List<RoomTypeModel>>("api/v1/room/roomtypes") ?? new();
        }
    }
}
