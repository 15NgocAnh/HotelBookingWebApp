using AutoMapper;
using HotelBooking.Domain.AggregateModels.AmenityAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
    {
        public AmenityRepository(
            AppDbContext context, 
            IMapper mapper, 
            IUnitOfWork unitOfWork) 
            : base(context, mapper, unitOfWork)
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