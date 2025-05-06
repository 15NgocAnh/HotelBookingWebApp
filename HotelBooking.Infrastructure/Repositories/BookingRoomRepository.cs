using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Repository;

namespace HotelBooking.Infrastructure.Repositories
{
    public class BookingRoomRepository : GenericRepository<BookingRoom>, IBookingRoomRepository
    {
        public BookingRoomRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
