using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetAllRooms
{
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, Result<List<RoomDto>>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;

        public GetAllRoomsQueryHandler(
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

        public async Task<Result<List<RoomDto>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Lấy danh sách phòng với filter theo hotelId
                var rooms = await _roomRepository.GetAllAsync();

                // Nếu có filter theo hotelIds
                if (request.HotelIds != null && request.HotelIds.Any())
                {
                    // Lấy danh sách buildingIds của các hotel
                    var buildingIds = await _buildingRepository.GetBuildingIdsByHotelIdsAsync(request.HotelIds);

                    // Lấy danh sách floorIds của các building
                    var floorIds = await _buildingRepository.GetFloorIdsByBuildingIdsAsync(buildingIds);

                    // Filter phòng theo floorIds
                    rooms = rooms.Where(r => floorIds.Contains(r.FloorId)).ToList();
                }

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