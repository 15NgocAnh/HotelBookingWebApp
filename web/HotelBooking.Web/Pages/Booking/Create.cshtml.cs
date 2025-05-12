using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Booking;
using HotelBooking.Domain.DTOs.Guest;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBooking.Web.Pages.Booking
{
    public class CreateModel : AbstractPageModel
    {
        public CreateModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public BookingType BookingType { get; set; }

        [BindProperty]
        public CreateBookingDTO Booking { get; set; }

        public async Task OnGetAsync(int roomId)
        {
            if (TempData["BookingData"] is string json && json != null)
            {
                Booking = JsonSerializer.Deserialize<CreateBookingDTO>(json);
            }
            else
            {
                var defaultGuest = await GetAsync<GuestDTO>("api/v1/guest/identity/000000000");
                var room = await GetAsync<RoomDTO>($"api/v1/rooms/{roomId}");

                // Initialize booking with room information
                Booking = new CreateBookingDTO
                {
                    EstCheckinTime = DateTime.Today.AddHours(14),        // Today at 14:00
                    EstCheckoutTime = DateTime.Today.AddDays(1).AddHours(12),  // Tomorrow at 12:00
                    Guest = defaultGuest ?? new(),
                    Rooms = room != null ? new List<RoomDTO>()
                {
                    room
                } : new List<RoomDTO>(),
                };
            }
        }

        public IActionResult OnPostUpdateGuestInfo()
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(Booking.GuestFullName))
            {
                ModelState.AddModelError("Booking.GuestFullName", "Họ tên khách không được bỏ trống.");
            }

            if (string.IsNullOrWhiteSpace(Booking.GuestIdentityNumber))
            {
                ModelState.AddModelError("Booking.GuestIdentityNumber", "Số CMND/CCCD không được bỏ trống");
            }

            // N?u có l?i, tr? l?i trang v?i thông tin l?i
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Gán vào Guest object
            Booking.Guest = new GuestDTO
            {
                FullName = Booking.GuestFullName,
                IdentityNumber = Booking.GuestIdentityNumber,
                IdentityIssueDate = Booking.GuestIdentityIssueDate,
                IdentityIssuePlace = Booking.GuestIdentityIssuePlace,
                Address = Booking.GuestAddress,
                Nationality = Booking.GuestNationality,
                Gender = Booking.GuestGender,
                BirthDate = Booking.GuestBirthDate,
                Province = Booking.GuestProvince,
                Phone = Booking.GuestPhone,
                Email = Booking.GuestEmail
            };
            return Page();
        }

        public async Task<IActionResult> OnPostResetGuestInfoAsync()
        {
            var defaultGuest = await GetAsync<GuestDTO>($"api/v1/guest/identity/{000000000}");
            Booking.Guest = defaultGuest ?? new();

            Booking.GuestFullName = null;
            Booking.GuestIdentityNumber = null;
            Booking.GuestIdentityIssueDate = default;
            Booking.GuestIdentityIssuePlace = null;
            Booking.GuestAddress = null;
            Booking.GuestNationality = null;
            Booking.GuestGender = default;
            Booking.GuestBirthDate = default;
            Booking.GuestProvince = null;
            Booking.GuestPhone = null;
            Booking.GuestEmail = null;

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await PostAsync<CreateBookingDTO, BookingDTO>("api/v1/booking", Booking);
                if (response != null)
                {
                    return RedirectToPage("/Booking/Index");
                }

                ModelState.AddModelError(string.Empty, "Error creating booking. Please try again.");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }

        public IActionResult OnPostBack()
        {
            return RedirectToPage("Index");
        }

        public IActionResult OnPostSelectRoom()
        {
            var bookingJson = JsonSerializer.Serialize(Booking); // Booking là model cần lưu
            TempData["BookingData"] = bookingJson;

            return RedirectToPage("SelectRoom");
        }
    }

    public class BookingModel
    {
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerIdNumber { get; set; }
        public decimal DepositAmount { get; set; }
        public string Notes { get; set; }
    }
}
