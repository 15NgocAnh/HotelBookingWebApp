using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context, IUnitOfWork unitOfWork) 
            : base(context, unitOfWork)
        {
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

        public async Task<Role> GetRolesByUserIdAsync(int userId)
        {
            var sql = @"
                    SELECT r.* 
                    FROM Roles r
                    JOIN Users u ON r.Id = u.RoleId
                    WHERE u.Id = {0} AND r.IsDeleted = 0";

            return await _context.Roles
                .FromSqlRaw(sql, userId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return !await _context.Set<Role>()
                .AnyAsync(r => r.Name == name && !r.IsDeleted);
        }
    }
}
