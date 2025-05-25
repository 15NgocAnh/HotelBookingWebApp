using HotelBooking.Application.CQRS.Dashboard.Dtos;

namespace HotelBooking.Application.CQRS.Dashboard.Queries
{
    public class GetDashboardStatisticsQuery : IQuery<Result<DashboardStatisticsDto>>
    {
    }
}