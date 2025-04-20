using System.Linq.Expressions;

namespace HotelBooking.Domain.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> Data);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> Data);
        Task SoftDeleteAsync(T entity);
        IQueryable<T> GetAllQueryable();
        Task<IQueryable<T>> GetSoftDeleteAsync();
    }
}
