using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBooking.Web.Pages.Booking
{
    public class SelectRoomModel : AbstractPageModel
    {
        public SelectRoomModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<RoomDTO> Rooms { get; set; } = new();
        public Dictionary<string, List<RoomDTO>> RoomsByFloor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get all rooms
            Rooms = await GetAsync<List<RoomDTO>>("api/v1/rooms");
            if (Rooms == null)
            {
                return NotFound();
            }

            // Get all floors
            var floors = await GetAsync<List<FloorDTO>>("api/v1/floors");
            if (floors == null)
            {
                return NotFound();
            }

            // Group rooms by floor name
            RoomsByFloor = Rooms
                .GroupBy(r => floors.FirstOrDefault(f => f.Id == r.FloorId)?.Name ?? "Unknown Floor")
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(r => r.RoomNumber).ToList()
                );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Get the selected room numbers from the form
            var selectedRoomIds = Request.Form["SelectedRooms"].ToList();
            
            if (selectedRoomIds.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một phòng.");
                // Load all rooms for the page to redisplay
                Rooms = await GetAsync<List<RoomDTO>>("api/v1/rooms");
                
                // Get all floors for room grouping
                var floors = await GetAsync<List<FloorDTO>>("api/v1/floors");
                
                // Group rooms by floor name
                RoomsByFloor = Rooms
                    .GroupBy(r => floors.FirstOrDefault(f => f.Id == r.FloorId)?.Name ?? "Unknown Floor")
                    .ToDictionary(
                        g => g.Key,
                        g => g.OrderBy(r => r.RoomNumber).ToList()
                    );
                    
                return Page();
            }

            if (TempData["BookingData"] is string tempDataJson)
            {
                // Load all rooms first
                Rooms = await GetAsync<List<RoomDTO>>("api/v1/rooms");
                
                var booking = JsonSerializer.Deserialize<CreateBookingDTO>(tempDataJson);
                
                // Get selected rooms by room ids
                var selectedRooms = new List<RoomDTO>();
                foreach (var roomId in selectedRoomIds)
                {
                    var room = Rooms.FirstOrDefault(r => r.Id == int.Parse(roomId));
                    if (room != null)
                    {
                        selectedRooms.Add(room);
                    }
                }
                
                // Update booking with selected rooms
                booking.Rooms = selectedRooms;
                
                // Save updated booking back to TempData
                TempData["BookingData"] = JsonSerializer.Serialize(booking);
            }

            return RedirectToPage("Create");
        }
    }
}
