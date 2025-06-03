using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetAllRoomAvailable
{
    public class GetAllRoomsAvailableQueryHandler : IRequestHandler<GetAllRoomsAvailableQuery, Result<List<RoomDto>>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;

        public GetAllRoomsAvailableQueryHandler(
            IRoomRepository roomRepository,
            IBuildingRepository buildingRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<RoomDto>>> Handle(GetAllRoomsAvailableQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rooms = await _roomRepository.GetAllRoomsAvailableAsync();

                // N?u c� filter theo hotelIds
                if (request.HotelIds != null && request.HotelIds.Any())
                {
                    // L?y danh s�ch buildingIds c?a c�c hotel
                    var buildingIds = await _buildingRepository.GetBuildingIdsByHotelIdsAsync(request.HotelIds);

                    // L?y danh s�ch floorIds c?a c�c building
                    var floorIds = await _buildingRepository.GetFloorIdsByBuildingIdsAsync(buildingIds);

                    // Filter ph�ng theo floorIds
                    rooms = rooms.Where(r => floorIds.Contains(r.FloorId)).ToList();
                }

                var roomDtos = _mapper.Map<List<RoomDto>>(rooms);
                return Result<List<RoomDto>>.Success(roomDtos);
            }
            catch (Exception ex)
            {
                return Result<List<RoomDto>>.Failure($"Failed to get rooms: {ex.Message}");
            }
        }
    }
}