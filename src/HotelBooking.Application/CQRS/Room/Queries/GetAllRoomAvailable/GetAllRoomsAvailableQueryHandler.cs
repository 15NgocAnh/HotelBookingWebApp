using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetAllRoomAvailable
{
    public class GetAllRoomsAvailableQueryHandler : IRequestHandler<GetAllRoomsAvailableQuery, Result<List<RoomDto>>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public GetAllRoomsAvailableQueryHandler(
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<RoomDto>>> Handle(GetAllRoomsAvailableQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rooms = await _roomRepository.GetAllRoomsAvailableAsync();
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