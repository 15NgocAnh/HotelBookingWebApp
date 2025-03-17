using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Repository.Interfaces;

namespace HotelBooking.Domain.Repository
{
    public class RoomTypeRepository : GenericRepository<RoomTypeModel>, IRoomTypeRepository
    {
        public RoomTypeRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
