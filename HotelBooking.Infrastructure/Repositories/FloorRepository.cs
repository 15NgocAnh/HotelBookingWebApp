using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces.Repositories;

namespace HotelBooking.Domain.Repository
{
    public class FloorRepository : GenericRepository<Floor>, IFloorRepository
    {
        public FloorRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
