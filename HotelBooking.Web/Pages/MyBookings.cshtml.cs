using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.Web.Pages
{
    public class MyBookingsModel : AbstractPageModel
    {
        public IEnumerable<BookingDTO> Bookings { get; set; }
        public string ErrorMessage { get; set; }

        public MyBookingsModel(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
            Bookings = new List<BookingDTO>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login", new { returnUrl = "/MyBookings" });
            }

            try
            {
                // Get current user ID
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    ErrorMessage = "User information not found.";
                    return Page();
                }

                // Get user's bookings
                var bookings = await GetAsync<IEnumerable<BookingDTO>>($"api/v1/booking/guest/{userId}");
                if (bookings == null)
                {
                    ErrorMessage = "Failed to retrieve bookings.";
                    return Page();
                }

                // Sort bookings by check-in date (newest first)
                Bookings = bookings.OrderByDescending(b => b.ArrivalDate);
                return Page();
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = $"Network error: {ex.Message}";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }

        public async Task<decimal> GetTotalPrice(BookingDTO booking)
        {
            try
            {
                // Get room details
                var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{booking.RoomId}");
                if (roomDetails == null)
                {
                    return 0;
                }

                // Calculate total days
                var totalDays = (int)(booking.DepartureDate - booking.ArrivalDate).TotalDays;
                return totalDays * roomDetails.PricePerNight;
            }
            catch
            {
                return 0;
            }
        }
    }
} 