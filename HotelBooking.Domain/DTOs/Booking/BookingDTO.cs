using HotelBooking.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.DTOs.Booking
{
    public class BookingDTO
    {
        public int BookingID { get; set; }
        public int GuestID { get; set; }
        public int RoomId { get; set; }
        public DateTime BookingDateTime { get; set; }

        [Required(ErrorMessage = "Check-in date is required")]
        [Display(Name = "Check-in Date")]
        public DateTime ArrivalDate { get; set; }

        [Required(ErrorMessage = "Check-out date is required")]
        [Display(Name = "Check-out Date")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Expected check-in time is required")]
        [Display(Name = "Expected Check-in Time")]
        public TimeSpan EstArrivalTime { get; set; }

        [Required(ErrorMessage = "Expected check-out time is required")]
        [Display(Name = "Expected Check-out Time")]
        public TimeSpan EstDepartureTime { get; set; }

        [Required(ErrorMessage = "Number of adults is required")]
        [Range(1, 10, ErrorMessage = "Number of adults must be between 1 and 10")]
        [Display(Name = "Number of Adults")]
        public int NumAdults { get; set; }

        [Required(ErrorMessage = "Number of children is required")]
        [Range(0, 10, ErrorMessage = "Number of children must be between 0 and 10")]
        [Display(Name = "Number of Children")]
        public int NumChildren { get; set; }

        [Display(Name = "Special Requirements")]
        public string? SpecialReq { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookingStatus Status { get; set; }
    }
} 