using HotelBooking.Domain.AggregateModels.InvoiceAggregate;

namespace HotelBooking.Infrastructure.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(AppDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<Invoice> GetByIdWithItemsAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice> GetByBookingIdAsync(int bookingId)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.BookingId == bookingId);
        }

        public async Task<IEnumerable<Invoice>> GetPendingInvoicesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Invoices
                .Include(i => i.Items)
                .Where(i => i.Status == InvoiceStatus.Pending);

            if (fromDate.HasValue)
                query = query.Where(i => i.DueDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(i => i.DueDate <= toDate.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Invoices
                .Include(i => i.Items)
                .Where(i => i.Status == InvoiceStatus.Overdue);

            if (fromDate.HasValue)
                query = query.Where(i => i.DueDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(i => i.DueDate <= toDate.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> HasPendingInvoiceForBookingAsync(int bookingId)
        {
            return await _context.Invoices
                .AnyAsync(i => i.BookingId == bookingId && i.Status == InvoiceStatus.Pending);
        }

        public async Task<decimal> GetMonthlyRevenueBookingAsync(int currentMonth, int currentYear)
        {
            return await _context.Invoices.Where(
                b => b.CreatedAt.Month == currentMonth 
                && b.CreatedAt.Year == currentYear 
                && !b.IsDeleted && 
                b.Status == InvoiceStatus.Paid)
                .SumAsync(b => b.TotalAmount);
        }

        public async Task<List<(DateTime CreatedAt, decimal TotalAmount)>> GetInvoiceSummariesAsync(
            Expression<Func<Invoice, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await _context.Invoices
                .Where(predicate)
                .Select(i => new { i.CreatedAt, i.TotalAmount })
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ContinueWith(t => t.Result.Select(x => (x.CreatedAt, x.TotalAmount)).ToList(), cancellationToken);
        }
    }
} 