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
    }
}
