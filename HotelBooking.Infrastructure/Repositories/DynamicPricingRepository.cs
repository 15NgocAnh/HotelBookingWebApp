using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class DynamicPricingRepository : GenericRepository<DynamicPricingRule>, IDynamicPricingRepository
    {
        public DynamicPricingRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<DynamicPricingRule>> GetByRoomTypeIdAsync(int roomTypeId)
        {
            return await _context.DynamicPricingRules
                .Include(dpr => dpr.RoomType)
                .Include(dpr => dpr.HourlyPrices)
                .Include(dpr => dpr.ExtraCharges)
                .Where(dpr => dpr.RoomTypeId == roomTypeId && !dpr.IsDeleted)
                .ToListAsync();
        }

        public async Task<DynamicPricingRule> GetByPricingIdAsync(int pricingId)
        {
            return await _context.DynamicPricingRules
                .Include(dpr => dpr.RoomType)
                .Include(dpr => dpr.HourlyPrices)
                .Include(dpr => dpr.ExtraCharges)
                .Where(dpr => dpr.Id == pricingId && !dpr.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DynamicPricingRule>> GetActiveRulesForDateAsync(DateTime date)
        {
            return await _context.DynamicPricingRules
                .Include(dpr => dpr.RoomType)
                .Include(dpr => dpr.HourlyPrices)
                .Include(dpr => dpr.ExtraCharges)
                .Where(dpr => dpr.StartDate <= date && dpr.EndDate >= date && !dpr.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<DynamicPricingRule>> GetActiveRulesForDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.DynamicPricingRules
                .Include(dpr => dpr.RoomType)
                .Include(dpr => dpr.HourlyPrices)
                .Include(dpr => dpr.ExtraCharges)
                .Where(dpr => 
                    ((dpr.StartDate >= startDate && dpr.StartDate <= endDate) ||
                     (dpr.EndDate >= startDate && dpr.EndDate <= endDate) ||
                     (dpr.StartDate <= startDate && dpr.EndDate >= endDate)) &&
                    !dpr.IsDeleted)
                .ToListAsync();
        }
    }
} 