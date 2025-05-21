using HotelBooking.Domain.AggregateModels.AmenityAggregate;

namespace HotelBooking.Infrastructure.Repositories
{
    public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
    {
        public AmenityRepository(
            AppDbContext context, 
            IUnitOfWork unitOfWork) 
            : base(context, unitOfWork)
        {
        }

        public async Task<IEnumerable<Amenity>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Amenities
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var amenity = await GetByIdAsync(id);
            if (amenity != null)
            {
                _context.Amenities.Remove(amenity);
                await _context.SaveChangesAsync();
            }
        }
    }
} 