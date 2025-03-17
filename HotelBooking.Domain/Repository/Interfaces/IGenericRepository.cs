using System.Linq.Expressions;

namespace HotelBooking.Domain.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> Data);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> Data);
        Task SoftDelete(T entity);
        IQueryable<T> GetAllAsync();
        IQueryable<T> GetSoftDelete();
    }
}
