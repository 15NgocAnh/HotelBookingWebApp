using HotelBooking.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;
        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            modelBuilder.Entity<RoomModel>().HasData(
                new RoomModel
                {
                    Id = 1,
                    RoomNumber = "101",
                    RoomTypeId = 1,
                    BedCount = 1,
                    MaxOccupancy = 2,
                    PricePerNight = 500000,
                    Status = RoomStatus.Available,
                    FloorNumber = 1,
                    Area = 25,
                    Facilities = new List<Facility> { Facility.WiFi, Facility.TV, Facility.AirConditioning },
                    ImageUrl = "/images/room101.jpg",
                    Description = "A cozy single room with modern amenities.",
                    LastBookedDate = null
                },
                new RoomModel
                {
                    Id = 2,
                    RoomNumber = "102",
                    RoomTypeId = 2,
                    BedCount = 2,
                    MaxOccupancy = 4,
                    PricePerNight = 800000,
                    Status = RoomStatus.Available,
                    FloorNumber = 1,
                    Area = 35,
                    Facilities = new List<Facility> { Facility.WiFi, Facility.TV, Facility.MiniBar, Facility.Safe },
                    ImageUrl = "/images/room102.jpg",
                    Description = "Spacious double room with minibar and safe.",
                    LastBookedDate = null
                },
                new RoomModel
                {
                    Id = 3,
                    RoomNumber = "201",
                    RoomTypeId = 3,
                    BedCount = 2,
                    MaxOccupancy = 5,
                    PricePerNight = 1500000,
                    Status = RoomStatus.Booked,
                    FloorNumber = 2,
                    Area = 50,
                    Facilities = new List<Facility> { Facility.WiFi, Facility.TV, Facility.Bathtub, Facility.BreakfastIncluded },
                    ImageUrl = "/images/room201.jpg",
                    Description = "Luxury suite with bathtub and complimentary breakfast.",
                    LastBookedDate = DateTime.UtcNow.AddDays(-3) // Last booked 3 days ago
                }
            );


            modelBuilder.Entity<RoomTypeModel>().HasData(
                new RoomTypeModel { Id = 1, Name = "Single", RoomPrice = 500000, DefaultRoomPrice = 450000, RoomImageUrl = "/images/single.jpg", RoomDescription = "A cozy single room." },
                new RoomTypeModel { Id = 2, Name = "Double", RoomPrice = 800000, DefaultRoomPrice = 750000, RoomImageUrl = "/images/double.jpg", RoomDescription = "A comfortable double room." },
                new RoomTypeModel { Id = 3, Name = "Suite", RoomPrice = 1500000, DefaultRoomPrice = 1400000, RoomImageUrl = "/images/suite.jpg", RoomDescription = "A luxurious suite." }
            );
        }
    }
}
