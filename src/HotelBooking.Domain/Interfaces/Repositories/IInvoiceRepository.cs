using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using System.Linq.Expressions;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<decimal> GetMonthlyRevenueBookingAsync(int currentMonth, int currentYear);
        Task<Invoice> GetByBookingIdAsync(int bookingId);
        Task<IEnumerable<Invoice>> GetPendingInvoicesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<bool> HasPendingInvoiceForBookingAsync(int bookingId);
        Task<List<(DateTime CreatedAt, decimal TotalAmount)>> GetInvoiceSummariesAsync(
            Expression<Func<Invoice, bool>> predicate,
            CancellationToken cancellationToken);
    }
} 