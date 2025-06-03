using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetRoomById
{
    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, Result<RoomDto>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;

        public GetRoomByIdQueryHandler(
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

        public async Task<Result<RoomDto>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(request.Id);
                if (room == null)
                {
                    return Result<RoomDto>.Failure("Room not found");
                }

                var roomDto = _mapper.Map<RoomDto>(room);

                var roomType = await _roomTypeRepository.GetByIdAsync(roomDto.RoomTypeId);
                roomDto.RoomTypeName = roomType?.Name ?? string.Empty;
                roomDto.RoomTypePrice = roomType?.Price ?? 0;

                var floor = await _buildingRepository.GetFloorByIdAsync(roomDto.FloorId);
                roomDto.FloorName = floor?.Name ?? string.Empty;

                var building = await _buildingRepository.GetBuildingByFloorIdAsync(roomDto.FloorId);
                roomDto.HotelId = building.HotelId;

                // Kiểm tra quyền truy cập hotel
                if (request.HotelIds != null && request.HotelIds.Any() && !request.HotelIds.Contains(building.HotelId))
                {
                    return Result<RoomDto>.Failure("Access denied: Room does not belong to your hotels.");
                }

                return Result<RoomDto>.Success(roomDto);
            }
            catch (Exception ex)
            {
                return Result<RoomDto>.Failure($"Failed to get room: {ex.Message}");
            }
        }
    }
}