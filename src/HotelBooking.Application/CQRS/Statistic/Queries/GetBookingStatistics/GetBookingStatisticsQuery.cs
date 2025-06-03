using HotelBooking.Application.CQRS.Statistic.Dtos;

namespace HotelBooking.Application.CQRS.Statistic.Queries.GetBookingStatistics
{
    public record GetBookingStatisticsQuery : IQuery<Result<BookingStatisticsDto>>
    {
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public List<int> HotelIds { get; set; } = new();
    }
}