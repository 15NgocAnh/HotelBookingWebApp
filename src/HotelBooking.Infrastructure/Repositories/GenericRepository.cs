namespace HotelBooking.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        protected GenericRepository(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _unitOfWork.SaveEntitiesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _unitOfWork.SaveEntitiesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _unitOfWork.SaveEntitiesAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _unitOfWork.SaveEntitiesAsync();
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _unitOfWork.SaveEntitiesAsync();
        }

        public virtual async Task SoftDeleteAsync(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("IsDeleted");
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(entity, true);
                await _unitOfWork.SaveEntitiesAsync();
            }
        }

        public virtual async Task<IQueryable<T>> GetSoftDeleteAsync()
        {
            return await Task.FromResult(_context.Set<T>().IgnoreQueryFilters().Where(e => EF.Property<DateTime?>(e, "DeletedAt") != null));
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AnyAsync(expression);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().CountAsync(expression);
        }
    }
}
