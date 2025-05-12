using HotelBooking.Domain.Common;
using System.Linq.Expressions;

namespace HotelBooking.Application.Common.Interfaces;
public interface IReadRepository<TEntity> where TEntity : IEntity
{
    Task<TEntity?> FindByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : struct, IEquatable<TId>;
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
