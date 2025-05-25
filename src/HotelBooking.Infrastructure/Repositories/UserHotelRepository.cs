using HotelBooking.Domain.AggregateModels.HotelAggregate;
using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Infrastructure.Repositories
{
    public class UserHotelRepository : GenericRepository<UserHotel>, IUserHotelRepository
    {
        public UserHotelRepository(AppDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task DeleteAllByUserIdAsync(int userId)
        {
            var userHotels = await _context.UserHotels
                .Where(uh => uh.UserId == userId)
                .ToListAsync();

            _context.UserHotels.RemoveRange(userHotels);
        }

        public async Task<List<Hotel>> GetAllByUserAsync(int userId)
        {
            return await _context.UserHotels
                .Include(uh => uh.Hotel)
                .Where(uh => uh.UserId == userId && !uh.IsDeleted && !uh.Hotel.IsDeleted)
                .Select(uh => uh.Hotel)
                .ToListAsync();
        }
    }
}
