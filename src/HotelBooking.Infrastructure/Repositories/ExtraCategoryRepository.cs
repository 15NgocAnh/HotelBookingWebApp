using AutoMapper;
using HotelBooking.Domain.AggregateModels.ExtraCategoryAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class ExtraCategoryRepository : GenericRepository<ExtraCategory>, IExtraCategoryRepository
    {
        public ExtraCategoryRepository(
            AppDbContext context, 
            IMapper mapper, 
            IUnitOfWork unitOfWork) 
            : base(context, mapper, unitOfWork)
        {
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return !await _context.ExtraCategories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> HasItemsAsync(int categoryId)
        {
            return await _context.ExtraItems
                .AnyAsync(i => i.ExtraCategoryId == categoryId);
        }

        public async Task DeleteAsync(int id)
        {
            var extraCategory = await GetByIdAsync(id);
            if (extraCategory != null)
            {
                _context.ExtraCategories.Remove(extraCategory);
                await _context.SaveChangesAsync();
            }
        }
    }
} 