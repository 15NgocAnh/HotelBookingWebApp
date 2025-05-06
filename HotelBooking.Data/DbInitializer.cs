using HotelBooking.Data.Models;
using HotelBooking.Domain.Entities;
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
            modelBuilder.Entity<GuestModel>().HasData(
                new GuestModel
                {
                    Id = 1,
                    FullName = "Khách lẻ",
                    IdentityNumber = "000000000",
                    Address = "N/A",
                    Nationality = "Vietnamese",
                    Gender = Gender.Other,
                    Phone = "0000000000",
                    Email = "anonymous@guest.local",
                    IdentityIssueDate = null,
                    IdentityIssuePlace = null,
                    BirthDate = null,
                    Province = null,
                    Bookings = null
                }
             );
        }
    }
}
