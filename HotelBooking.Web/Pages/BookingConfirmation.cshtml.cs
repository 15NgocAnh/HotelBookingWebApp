using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Security.Claims;
using HotelBooking.Data.Models;

namespace HotelBooking.Web.Pages
{
    public class BookingConfirmationModel : AbstractPageModel
    {
        private readonly IMapper _mapper;

        [BindProperty(SupportsGet = true)]
        public int BookingId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Action { get; set; }

        public BookingDTO Booking { get; set; }
        public RoomDTO Room { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalDays { get; set; }
        public decimal TotalPrice { get; set; }

        public BookingConfirmationModel(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login", new { returnUrl = $"/BookingConfirmation?bookingId={BookingId}" });
            }

            if (BookingId <= 0)
            {
                ErrorMessage = "Invalid booking ID.";
                return Page();
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

                // Get booking details
                var bookingDetails = await GetAsync<BookingDTO>($"api/v1/booking/{BookingId}");
                if (bookingDetails == null)
                {
                    ErrorMessage = "Booking not found.";
                    return Page();
                }

                // Check if the booking belongs to the current user
                if (bookingDetails.GuestID != int.Parse(userId))
                {
                    ErrorMessage = "You don't have permission to view this booking.";
                    return Page();
                }

                Booking = bookingDetails;

                // Get room details
                var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{Booking.RoomId}");
                if (roomDetails == null)
                {
                    ErrorMessage = "Room information not found.";
                    return Page();
                }

                Room = _mapper.Map<RoomDTO>(roomDetails);

                // Calculate total days and price
                TotalDays = (int)(Booking.DepartureDate - Booking.ArrivalDate).TotalDays;
                TotalPrice = TotalDays * Room.PricePerNight;

                // Handle cancellation if requested
                if (Action?.ToLower() == "cancel" && Booking.Status == BookingStatus.Pending)
                {
                    try
                    {
                        await PostAsync<object, object>($"api/v1/booking/{BookingId}/cancel", null);
                        // Refresh booking details after cancellation
                        Booking = await GetAsync<BookingDTO>($"api/v1/booking/{BookingId}");
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = $"Failed to cancel booking: {ex.Message}";
                    }
                }

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
    }
} 