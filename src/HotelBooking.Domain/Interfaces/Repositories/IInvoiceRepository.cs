using HotelBooking.Domain.AggregateModels.InvoiceAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<Invoice> GetByIdWithItemsAsync(int id);
        Task<Invoice> GetByBookingIdAsync(int bookingId);
        Task<IEnumerable<Invoice>> GetPendingInvoicesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<bool> HasPendingInvoiceForBookingAsync(int bookingId);
    }
} 