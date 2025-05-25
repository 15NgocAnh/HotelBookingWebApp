using HotelBooking.Application.CQRS.Statistic.Dtos;

namespace HotelBooking.Application.CQRS.Statistic.Queries.GetRevenueStatistics
{
    public class GetRevenueStatisticsQuery : IQuery<Result<RevenueStatisticsDto>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}