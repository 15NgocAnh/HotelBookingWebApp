using HotelBooking.Domain.AggregateModels.BedTypeAggregate;

namespace HotelBooking.Infrastructure.Repositories
{
    public class BedTypeRepository : GenericRepository<BedType>, IBedTypeRepository
    {
        public BedTypeRepository(
            AppDbContext context, 
            IUnitOfWork unitOfWork) 
            : base(context, unitOfWork)
        {
        }

        public async Task<IEnumerable<BedType>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.BedTypes
                .Where(b => ids.Contains(b.Id))
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var bedType = await GetByIdAsync(id);
            if (bedType != null)
            {
                _context.BedTypes.Remove(bedType);
                await _context.SaveChangesAsync();
            }
        }
    }
} 