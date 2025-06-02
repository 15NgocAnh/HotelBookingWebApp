using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context, IUnitOfWork unitOfWork) 
        : base(context, unitOfWork)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return !await _context.Set<User>()
            .AnyAsync(u => u.Email == email && !u.IsDeleted);
    }
}
