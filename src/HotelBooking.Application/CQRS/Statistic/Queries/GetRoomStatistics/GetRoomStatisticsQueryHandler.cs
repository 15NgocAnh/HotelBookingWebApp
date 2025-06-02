using HotelBooking.Application.CQRS.Statistic.Dtos;
using HotelBooking.Domain.AggregateModels.RoomAggregate;

namespace HotelBooking.Application.CQRS.Statistic.Queries.GetRoomStatistics
{
    public class GetRoomStatisticsQueryHandler : IRequestHandler<GetRoomStatisticsQuery, Result<RoomStatisticsDto>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetRoomStatisticsQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Result<RoomStatisticsDto>> Handle(GetRoomStatisticsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalRooms = await _roomRepository.CountAsync(r => !r.IsDeleted);
                var availableRooms = await _roomRepository.CountAsync(r => r.Status == RoomStatus.Available && !r.IsDeleted);
                var bookedRooms = await _roomRepository.CountAsync(r => r.Status == RoomStatus.Booked && !r.IsDeleted);
                var cleaningUpRooms = await _roomRepository.CountAsync(r => r.Status == RoomStatus.CleaningUp && !r.IsDeleted);
                var maintenanceRooms = await _roomRepository.CountAsync(r => r.Status == RoomStatus.UnderMaintenance && !r.IsDeleted);
                var roomTypeStatistics = await _roomRepository.GetRoomTypeStatisticsAsync();
                var roomStatusStatistics = await _roomRepository.GetRoomStatusStatisticsAsync();

                var statistics = new RoomStatisticsDto
                {
                    TotalRooms = totalRooms,
                    AvailableRooms = availableRooms,
                    BookedRooms = bookedRooms,
                    CleaningUpRooms = cleaningUpRooms,
                    MaintenanceRooms = maintenanceRooms,
                    RoomTypeStatistics = roomTypeStatistics.Select(x => new RoomTypeStatisticsDto
                    {
                        RoomType = x.RoomType,
                        Total = x.Total,
                        Available = x.Available,
                        Booked = x.Booked,
                        CleaningUp = x.CleaningUp,
                        Maintenance = x.UnderMaintenance
                    }).ToList(),
                    RoomStatusStatistics = roomStatusStatistics.Select(x => new RoomStatusStatisticsDto
                    {
                        Status = x.Status,
                        Count = x.Count
                    }).ToList()
                };

                return Result<RoomStatisticsDto>.Success(statistics);
            }
            catch (Exception ex)
            {
                return Result<RoomStatisticsDto>.Failure("Something went wrong", ex);
            }
        }
    }
}