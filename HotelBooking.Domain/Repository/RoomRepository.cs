using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Repository.Interfaces;

namespace HotelBooking.Domain.Repository
{
    public class RoomRepository : GenericRepository<RoomModel>, IRoomRepository
    {
        public RoomRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
