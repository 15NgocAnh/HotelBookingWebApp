namespace HotelBooking.Application.CQRS.Room.Commands
{
    public record CreateRoomCommand : ICommand<Result<int>>
    {
        public string Name { get; init; }
        public int FloorId { get; init; }
        public int RoomTypeId { get; init; }
    }
} 