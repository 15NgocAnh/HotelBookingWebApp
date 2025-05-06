using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Booking
{
    [Authorize]
    public class IndexModel : AbstractPageModel
    {
        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<RoomDTO> Rooms { get; set; } = new();
        public Dictionary<string, List<RoomDTO>> RoomsByFloor { get; set; } = new();
        public List<BookingDTO> Bookings { get; set; }

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

            //Bookings = await GetAsync<List<BookingDTO>>("api/v1/bookings");

            return Page();
        }

        public async Task<IActionResult> OnPostCheckInAsync(int bookingId)
        {
            try
            {
                var response = await PostAsync<int, bool>($"api/v1/bookings/{bookingId}/checkin", bookingId);
                if (response)
                {
                    return RedirectToPage();
                }

                ModelState.AddModelError(string.Empty, "Error checking in. Please try again.");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCheckOutAsync(int bookingId)
        {
            try
            {
                var response = await PostAsync<int, bool>($"api/v1/bookings/{bookingId}/checkout", bookingId);
                if (response)
                {
                    return RedirectToPage();
                }

                ModelState.AddModelError(string.Empty, "Error checking out. Please try again.");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }
    }
}
