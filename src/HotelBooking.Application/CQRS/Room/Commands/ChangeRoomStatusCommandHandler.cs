using HotelBooking.Application.Common.Exceptions;

namespace HotelBooking.Application.CQRS.Room.Commands;

public class ChangeRoomStatusCommandHandler : IRequestHandler<ChangeRoomStatusCommand, Result>
{
    private readonly IRoomRepository _roomRepository;

    public ChangeRoomStatusCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result> Handle(ChangeRoomStatusCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.Id);

        if (room == null)
        {
            throw new NotFoundException($"Room with ID {request.Id} not found.");
        }

        room.UpdateStatus(request.Status);
        await _roomRepository.UpdateAsync(room);

        return Result.Success();
    }
} 