using AutoMapper;
using HotelBooking.Domain.AggregateModels.ExtraItemAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class ExtraItemRepository : GenericRepository<ExtraItem>, IExtraItemRepository
    {
        public ExtraItemRepository(AppDbContext context, IMapper mapper, IUnitOfWork unitOfWork) : base(context, mapper, unitOfWork)
        {
        }

        public async Task<bool> IsNameUniqueInCategoryAsync(int categoryId, string name)
        {
            return !await _context.ExtraItems
                .AnyAsync(i => i.ExtraCategoryId == categoryId && 
                              i.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsInUseAsync(int itemId)
        {
            // Check if item is used in any booking extras
            return await _context.Bookings
                .AnyAsync(b => b.ExtraUsages.Any(eu => eu.ExtraItemId == itemId));
        }

        public async Task DeleteAsync(int id)
        {
            var extraItem = await GetByIdAsync(id);
            if (extraItem != null)
            {
                _context.ExtraItems.Remove(extraItem);
                await _context.SaveChangesAsync();
            }
        }
    }
} 