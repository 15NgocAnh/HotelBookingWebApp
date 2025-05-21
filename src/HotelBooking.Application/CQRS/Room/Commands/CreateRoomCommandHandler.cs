namespace HotelBooking.Application.CQRS.Room.Commands
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Result<int>>
    {
        private readonly IRoomRepository _roomRepository;

        public CreateRoomCommandHandler(
            IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Result<int>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if room number is unique in the building
                if (!await _roomRepository.IsRoomNumberUniqueInBuildingAsync(request.FloorId, request.Name))
                {
                    return Result<int>.Failure("Room number already exists in this building");
                }

                var room = new Domain.AggregateModels.RoomAggregate.Room(
                    request.Name,  
                    request.FloorId,  
                    request.RoomTypeId
                );

                await _roomRepository.AddAsync(room);
                return Result<int>.Success(room.Id);
            }
            catch (System.Exception ex)
            {
                return Result<int>.Failure($"Failed to create room: {ex.Message}");
            }
        }
    }
} 