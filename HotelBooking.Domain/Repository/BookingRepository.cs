using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Repository.Interfaces;

namespace HotelBooking.Domain.Repository
{
    public class BookingRepository : GenericRepository<BookingModel>, IBookingRepository
    {
        public BookingRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
