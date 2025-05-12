using AutoMapper;
using HotelBooking.Domain.AggregateModels.BedTypeAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class BedTypeRepository : GenericRepository<BedType>, IBedTypeRepository
    {
        private readonly AppDbContext _context;

        public BedTypeRepository(
            AppDbContext context, 
            IMapper mapper, 
            IUnitOfWork unitOfWork) 
            : base(context, mapper, unitOfWork)
        {
            _context = context;
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