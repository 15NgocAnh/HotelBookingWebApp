using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Interfaces.Repositories;

namespace HotelBooking.Domain.Repository
{
    public class BillRepository : GenericRepository<BillModel>, IBillRepository
    {
        public BillRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
