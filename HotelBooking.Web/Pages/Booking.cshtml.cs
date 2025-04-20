using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Security.Claims;

namespace HotelBooking.Web.Pages
{
    public class BookingModel : AbstractPageModel
    {
        private readonly IMapper _mapper;

        [BindProperty]
        public BookingDTO Booking { get; set; }

        [BindProperty(SupportsGet = true)]
        public int RoomId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? CheckInDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? CheckOutDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? AdultsCnt { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ChildrenCnt { get; set; }

        public RoomDTO Room { get; set; }
        public string ErrorMessage { get; set; }

        public BookingModel(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
            _mapper = mapper;
            Booking = new BookingDTO();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (RoomId <= 0)
            {
                return RedirectToPage("/Rooms");
            }

            // Get room details
            var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{RoomId}");
            if (roomDetails == null)
            {
                return NotFound();
            }

            // Convert RoomDetailsDTO to RoomDTO
            Room = _mapper.Map<RoomDTO>(roomDetails);

            if (Room.Status != "Available")
            {
                ErrorMessage = "This room is not available for booking.";
                return Page();
            }

            // Set default values
            Booking.RoomId = RoomId;
            Booking.BookingDateTime = DateTime.Now;
            Booking.Status = BookingStatus.Pending;

            // Set search parameters if provided
            if (CheckInDate.HasValue)
            {
                Booking.ArrivalDate = CheckInDate.Value.Date;
            }
            else
            {
                Booking.ArrivalDate = DateTime.Now.Date;
            }

            if (CheckOutDate.HasValue)
            {
                Booking.DepartureDate = CheckOutDate.Value.Date;
            }
            else
            {
                Booking.DepartureDate = DateTime.Now.Date.AddDays(1);
            }

            // Set guest counts with validation
            Booking.NumAdults = Math.Max(1, AdultsCnt ?? 1);
            Booking.NumChildren = Math.Max(0, ChildrenCnt ?? 0);

            // Set default arrival and departure times
            Booking.EstArrivalTime = new TimeSpan(14, 0, 0); // 2:00 PM
            Booking.EstDepartureTime = new TimeSpan(12, 0, 0); // 12:00 PM

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Get room details again for the view
                var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{RoomId}");
                if (roomDetails != null)
                {
                    Room = _mapper.Map<RoomDTO>(roomDetails);
                }
                return Page();
            }

            // Validate dates
            if (Booking.ArrivalDate >= Booking.DepartureDate)
            {
                ModelState.AddModelError("Booking.DepartureDate", "Check-out date must be after check-in date");
                var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{RoomId}");
                if (roomDetails != null)
                {
                    Room = _mapper.Map<RoomDTO>(roomDetails);
                }
                return Page();
            }

            if (Booking.ArrivalDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("Booking.ArrivalDate", "Check-in date cannot be in the past");
                var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{RoomId}");
                if (roomDetails != null)
                {
                    Room = _mapper.Map<RoomDTO>(roomDetails);
                }
                return Page();
            }

            // Get current user ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ErrorMessage = "Please log in to make a booking.";
                var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{RoomId}");
                if (roomDetails != null)
                {
                    Room = _mapper.Map<RoomDTO>(roomDetails);
                }
                return Page();
            }

            Booking.GuestID = int.Parse(userId);

            try
            {
                // Create booking
                var response = await PostAsync<BookingDTO, BookingDTO>("api/v1/booking", Booking);
                if (response == null)
                {
                    ErrorMessage = "Failed to create booking. Please try again.";
                    var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{RoomId}");
                    if (roomDetails != null)
                    {
                        Room = _mapper.Map<RoomDTO>(roomDetails);
                    }
                    return Page();
                }

                // Redirect to booking confirmation page
                return RedirectToPage("/BookingConfirmation", new { bookingId = response.BookingID });
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = $"Network error: {ex.Message}";
                var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{RoomId}");
                if (roomDetails != null)
                {
                    Room = _mapper.Map<RoomDTO>(roomDetails);
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating booking: {ex.Message}";
                var roomDetails = await GetAsync<RoomDetailsDTO>($"api/v1/room/{RoomId}");
                if (roomDetails != null)
                {
                    Room = _mapper.Map<RoomDTO>(roomDetails);
                }
                return Page();
            }
        }
    }
} 