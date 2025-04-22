using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.DTOs.Room
{
    public class RoomUpdateDTO
    {
        [Required(ErrorMessage = "Room number is required")]
        [StringLength(10, ErrorMessage = "Room number cannot exceed 10 characters")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "Room type is required")]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Price per night is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal PricePerNight { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Area is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Area must be greater than 0")]
        public double Area { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Maximum occupancy is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Maximum occupancy must be at least 1")]
        public int MaxOccupancy { get; set; }

        [Required(ErrorMessage = "Bed count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Bed count must be at least 1")]
        public int BedCount { get; set; }

        [Required(ErrorMessage = "Facilities are required")]
        public string Facilities { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
    }
} 