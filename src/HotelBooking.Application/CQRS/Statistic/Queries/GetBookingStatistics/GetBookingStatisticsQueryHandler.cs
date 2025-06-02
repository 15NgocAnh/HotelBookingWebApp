using HotelBooking.Application.CQRS.Statistic.Dtos;

namespace HotelBooking.Application.CQRS.Statistic.Queries.GetBookingStatistics
{
    public class GetBookingStatisticsQueryHandler : IRequestHandler<GetBookingStatisticsQuery, Result<BookingStatisticsDto>>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingStatisticsQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Result<BookingStatisticsDto>> Handle(GetBookingStatisticsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalBookings = await _bookingRepository.GetTotalBookingsAsync(request.StartDate, request.EndDate);
                var completedBookings = await _bookingRepository.GetCompletedBookingsAsync(request.StartDate, request.EndDate);
                var cancelledBookings = await _bookingRepository.GetCancelledBookingsAsync(request.StartDate, request.EndDate);
                var pendingBookings = await _bookingRepository.GetPendingBookingsAsync(request.StartDate, request.EndDate);
                var dailyBookings = await _bookingRepository.GetDailyBookingsAsync(request.StartDate, request.EndDate);
                var roomTypeBookings = await _bookingRepository.GetRoomTypeBookingsAsync(request.StartDate, request.EndDate);

                var statistics = new BookingStatisticsDto
                {
                    TotalBookings = totalBookings,
                    CompletedBookings = completedBookings,
                    CancelledBookings = cancelledBookings,
                    PendingBookings = pendingBookings,
                    DailyBookings = dailyBookings.Select(x => new DailyBookingDto
                    {
                        Date = x.Date,
                        Count = x.Count,
                        PendingCount = x.PendingCount,
                        CancelledCount = x.CancelledCount,
                        CompletedCount = x.CompletedCount
                    }).ToList(),
                    RoomTypeBookings = roomTypeBookings.Select(x => new RoomTypeBookingDto
                    {
                        RoomType = x.RoomType,
                        Count = x.BookedCount,
                        TotalRevenue = x.TotalRevenue
                    }).ToList()
                };

                return Result<BookingStatisticsDto>.Success(statistics);
            }
            catch (Exception ex)
            {
                return Result<BookingStatisticsDto>.Failure("Something went wrong", ex);
            }
        }
    }
}