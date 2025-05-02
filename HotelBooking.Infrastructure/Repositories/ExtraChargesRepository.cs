using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class ExtraChargesRepository : GenericRepository<ExtraCharge>, IExtraChargesRepository
    {
        public ExtraChargesRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task DeleteAllByDynamicPricingIdAsync(int id)
        {
            var extraCharges = await _context.ExtraCharges
                .Where(ec => ec.DynamicPricingRuleId == id)
                .ToListAsync();

            if (extraCharges.Count != 0)
            {
                _context.ExtraCharges.RemoveRange(extraCharges);
                await _context.SaveChangesAsync();
            }
        }
    }
}
