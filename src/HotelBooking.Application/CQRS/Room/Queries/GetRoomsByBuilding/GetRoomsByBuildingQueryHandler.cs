using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetRoomsByBuilding
{
    public class GetRoomsByBuildingQueryHandler : IRequestHandler<GetRoomsByBuildingQuery, Result<List<RoomDto>>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;

        public GetRoomsByBuildingQueryHandler(
            IRoomRepository roomRepository,
            IRoomTypeRepository roomTypeRepository,
            IBuildingRepository buildingRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<RoomDto>>> Handle(GetRoomsByBuildingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var building = await _buildingRepository.GetByIdAsync(request.BuildingId);
                if (building == null)
                {
                    return Result<List<RoomDto>>.Failure("Building not found.");
                }

                if (!request.HotelIds.Contains(building.HotelId))
                {
                    return Result<List<RoomDto>>.Failure("Access denied: Building does not belong to your hotels.");
                }

                var rooms = await _roomRepository.GetRoomsByBuildingAsync(request.BuildingId);
                var roomDtos = _mapper.Map<List<RoomDto>>(rooms);
                foreach (var roomDto in roomDtos)
                {
                    var roomType = await _roomTypeRepository.GetByIdAsync(roomDto.RoomTypeId);
                    roomDto.RoomTypeName = roomType?.Name ?? string.Empty;
                    roomDto.RoomTypePrice = roomType?.Price ?? 0;

                    var floor = await _buildingRepository.GetFloorByIdAsync(roomDto.FloorId);
                    roomDto.FloorName = floor?.Name ?? string.Empty;
                }

                return Result<List<RoomDto>>.Success(roomDtos);
            }
            catch (Exception ex)
            {
                return Result<List<RoomDto>>.Failure($"Failed to get rooms: {ex.Message}");
            }
        }
    }
}