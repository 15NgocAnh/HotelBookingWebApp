using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Repository.Interfaces;

namespace HotelBooking.Domain.Repository
{
    public class HotelRepository : GenericRepository<HotelModel>, IHotelRepository
    {
        public HotelRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
