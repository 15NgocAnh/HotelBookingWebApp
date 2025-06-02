using HotelBooking.Application.CQRS.Dashboard.Dtos;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.AggregateModels.RoomAggregate;

namespace HotelBooking.Application.CQRS.Dashboard.Queries
{
    public class GetDashboardStatisticsQueryHandler : IRequestHandler<GetDashboardStatisticsQuery, Result<DashboardStatisticsDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBookingRepository _bookingRepository;

        public GetDashboardStatisticsQueryHandler(
            IUserRepository userRepository,
            IRoomRepository roomRepository,
            IInvoiceRepository invoiceRepository,
            IBookingRepository bookingRepository)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _invoiceRepository = invoiceRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<Result<DashboardStatisticsDto>> Handle(GetDashboardStatisticsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var statistics = new DashboardStatisticsDto();

                // Get total users
                statistics.TotalUsers = await _userRepository.CountAsync(u => !u.IsDeleted);

                // Get monthly revenue
                var currentMonth = DateTime.UtcNow.Month;
                var currentYear = DateTime.UtcNow.Year;
                statistics.MonthlyRevenue = await _invoiceRepository.GetMonthlyRevenueBookingAsync(currentMonth, currentYear);

                // Get total bookings
                statistics.TotalBookings = await _bookingRepository.CountAsync(b => !b.IsDeleted && b.Status != Domain.AggregateModels.BookingAggregate.BookingStatus.Cancelled);

                // Get available rooms
                statistics.AvailableRooms = await _roomRepository.CountAsync(r => r.Status == RoomStatus.Available);

                // Get monthly revenue data for the last 6 months
                var sixMonthsAgo = DateTime.UtcNow.AddMonths(-5);
                var invoices = await _invoiceRepository.GetInvoiceSummariesAsync(b => b.CreatedAt >= sixMonthsAgo, cancellationToken);

                var monthlyRevenueData = invoices
                    .GroupBy(i => new { i.CreatedAt.Year, i.CreatedAt.Month })
                    .Select(g => new MonthlyRevenueDto
                    {
                        Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Revenue = g.Sum(i => i.TotalAmount)
                    })
                    .OrderBy(x => x.Month)
                    .ToList();

                statistics.MonthlyRevenueData = monthlyRevenueData;

                // Get room type statistics
                var rooms = await _roomRepository.GetRoomWithTypeNamesAsync(cancellationToken);

                var roomTypeStatistics = rooms
                    .GroupBy(r => r.RoomTypeName)
                    .Select(g => new RoomTypeStatisticsDto
                    {
                        RoomType = g.Key,
                        BookedCount = g.Count(r => r.Status == RoomStatus.Booked.ToString()),
                        AvailableCount = g.Count(r => r.Status == RoomStatus.Available.ToString())
                    })
                    .OrderBy(x => x.RoomType)
                    .ToList();

                statistics.RoomTypeStatistics = roomTypeStatistics;

                return Result<DashboardStatisticsDto>.Success(statistics);
            }
            catch (Exception ex)
            {
                return Result<DashboardStatisticsDto>.Failure("Some thing went wrong", ex);
            }
        }
    }
}