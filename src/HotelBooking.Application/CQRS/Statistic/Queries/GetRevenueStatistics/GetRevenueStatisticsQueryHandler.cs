using HotelBooking.Application.CQRS.Statistic.Dtos;

namespace HotelBooking.Application.CQRS.Statistic.Queries.GetRevenueStatistics
{
    public class GetRevenueStatisticsQueryHandler : IRequestHandler<GetRevenueStatisticsQuery, Result<RevenueStatisticsDto>>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetRevenueStatisticsQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Result<RevenueStatisticsDto>> Handle(GetRevenueStatisticsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalRevenue = await _bookingRepository.GetTotalRevenueAsync(request.StartDate, request.EndDate);
                var monthlyRevenue = await _bookingRepository.GetMonthlyRevenueAsync(request.StartDate, request.EndDate);
                var weeklyRevenue = await _bookingRepository.GetWeeklyRevenueAsync(request.StartDate, request.EndDate);
                var dailyRevenue = await _bookingRepository.GetDailyRevenueAsync(request.StartDate, request.EndDate);
                var monthlyRevenueData = await _bookingRepository.GetMonthlyRevenueDataAsync();

                var statistics = new RevenueStatisticsDto
                {
                    TotalRevenue = totalRevenue,
                    TotalMonthlyRevenue = monthlyRevenue,
                    TotalWeeklyRevenue = weeklyRevenue,
                    DailyRevenue = dailyRevenue.Select(x => new DailyRevenueDto
                    {
                        Date = x.Date,
                        Revenue = x.Revenue
                    }).ToList(),
                    MonthlyRevenue = monthlyRevenueData.Select(x => new MonthlyRevenueDto
                    {
                        Month = x.Month,
                        Revenue = x.Revenue,
                        BookingCount = x.BookingCount
                    }).ToList()
                };

                return Result<RevenueStatisticsDto>.Success(statistics);
            }
            catch (Exception ex)
            {
                return Result<RevenueStatisticsDto>.Failure("Something went wrong", ex);
            }
        }
    }
}