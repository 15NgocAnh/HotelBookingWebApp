using HotelBooking.Application.CQRS.Statistic.Dtos;

namespace HotelBooking.Application.CQRS.Statistic.Queries.GetRoomStatistics
{
    public class GetRoomStatisticsQuery : IQuery<Result<RoomStatisticsDto>>
    {
    }
}