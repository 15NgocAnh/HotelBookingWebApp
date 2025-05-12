using AutoMapper;
using HotelBooking.Domain.AggregateModels.UserAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context, IMapper mapper, IUnitOfWork unitOfWork) 
            : base(context, mapper, unitOfWork)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<bool> AssignRoleToUserAsync(int userId, int roleId)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Set<Role>()
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }

        public override async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted);
        }

        public Task<IEnumerable<Role>> GetRolesByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return !await _context.Set<Role>()
                .AnyAsync(r => r.Name == name && !r.IsDeleted);
        }

        public Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            throw new NotImplementedException();
        }
    }
}
