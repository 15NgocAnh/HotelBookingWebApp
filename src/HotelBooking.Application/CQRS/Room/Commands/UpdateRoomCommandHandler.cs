using HotelBooking.Domain.Common;

namespace HotelBooking.Application.CQRS.Room.Commands
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Result>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoomCommandHandler(
            IRoomRepository roomRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(request.Id);
                if (room == null)
                {
                    return Result.Failure("Room not found");
                }

                // Check if room number is unique in the building (excluding current room)
                if (!await _roomRepository.IsRoomNumberUniqueInBuildingAsync(request.FloorId, request.Name))
                {
                    var existingRoom = await _roomRepository.GetByRoomNumberAsync(request.Name);
                    if (existingRoom != null && existingRoom.Id != request.Id)
                    {
                        return Result.Failure("Room number already exists in this building");
                    }
                }

                // Update room name and room type
                room.Update(request.Name, request.RoomTypeId);

                await _roomRepository.UpdateAsync(room);

                return Result.Success();
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Failed to update room: {ex.Message}");
            }
        }
    }
} 