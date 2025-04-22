using HotelBooking.Data.Models;

namespace HotelBooking.Domain.DTOs.Room
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public string Status { get; set; }
        public double Area { get; set; }
        public string ImageUrl { get; set; }
        public int MaxOccupancy { get; set; }
        public int BedCount { get; set; }
        public List<Facility> Facilities { get; set; }
        public string Description { get; set; }
    }
}
