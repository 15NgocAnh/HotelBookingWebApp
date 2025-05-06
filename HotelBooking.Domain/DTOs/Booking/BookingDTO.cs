using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Guest;
using HotelBooking.Domain.DTOs.Room;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.DTOs.Booking
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int GuestID { get; set; }
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Check-in date is required")]
        [Display(Name = "Check-in Date")]
        public DateTime EstCheckinTime { get; set; }

        [Required(ErrorMessage = "Check-out date is required")]
        [Display(Name = "Check-out Date")]
        public DateTime EstCheckoutTime { get; set; }

        [Required(ErrorMessage = "Number of adults is required")]
        [Range(1, 10, ErrorMessage = "Number of adults must be between 1 and 10")]
        [Display(Name = "Number of Adults")]
        public int NumberOfAdults { get; set; }

        [Required(ErrorMessage = "Number of children is required")]
        [Range(0, 10, ErrorMessage = "Number of children must be between 0 and 10")]
        [Display(Name = "Number of Children")]
        public int NumberOfChildren { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookingStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookingType BookingType { get; set; }
        public string? Note { get; set; }
        public DateTime? ActualCheckInTime { get; set; }
        public DateTime? ActualCheckOutTime { get; set; }
        public GuestDTO Guest { get; set; }
    }

    public class CreateBookingDTO
    {
        public int Id { get; set; }
        public DateTime EstCheckinTime { get; set; }
        public DateTime EstCheckoutTime { get; set; }
        public int NumberOfAdults { get; set; } = 1;
        public int NumberOfChildren { get; set; } = 0;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookingType BookingType { get; set; }
        public string? Note { get; set; }

        [StringLength(100)]
        public string? GuestFullName { get; set; }

        [StringLength(20)]
        public string? GuestIdentityNumber { get; set; }

        public DateTime? GuestIdentityIssueDate { get; set; }

        [StringLength(100)]
        public string? GuestIdentityIssuePlace { get; set; }

        [StringLength(200)]
        public string? GuestAddress { get; set; }

        [StringLength(50)]
        public string? GuestNationality { get; set; }

        public Gender? GuestGender { get; set; }

        public DateTime? GuestBirthDate { get; set; }

        [StringLength(50)]
        public string? GuestProvince { get; set; }

        [StringLength(20)]
        public string? GuestPhone { get; set; }

        [StringLength(100)]
        public string? GuestEmail { get; set; }

        public GuestDTO Guest { get; set; } = new();

        public List<RoomDTO> Rooms { get; set; } = new();
    }
} 