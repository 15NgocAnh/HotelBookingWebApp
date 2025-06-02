namespace HotelBooking.Application.Common.Interfaces
{
    public interface IUnitOfWorkWithTransaction
    {
        Task ExecuteWithTransactionAsync(Func<Task> action, CancellationToken cancellationToken);
        Task<T> ExecuteWithTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken);
    }
}
