using AutoMapper;
using HotelBooking.Domain.AggregateModels.UserAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context, IMapper mapper, IUnitOfWork unitOfWork) 
        : base(context, mapper, unitOfWork)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Set<User>()
            .Where(u => !u.IsDeleted)
            .ToListAsync();
    }

    public override async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
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

    public async Task<User> GetByIdWithRolesAsync(int id)
    {
        return await _context.Set<User>()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName)
    {
        return await _context.Set<User>()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName))
            .ToListAsync();
    }
}
