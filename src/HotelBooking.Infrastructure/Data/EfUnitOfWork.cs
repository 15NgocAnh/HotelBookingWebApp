using HotelBooking.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Data
{
    public class EfUnitOfWork : IUnitOfWorkWithTransaction
    {
        private readonly AppDbContext _dbContext;

        public EfUnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteWithTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                await action();
                await transaction.CommitAsync(cancellationToken);
            });
        }

        public async Task<T> ExecuteWithTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                var result = await action();
                await transaction.CommitAsync(cancellationToken);
                return result;
            });
        }
    }
}
