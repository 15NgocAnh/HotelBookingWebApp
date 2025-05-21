namespace HotelBooking.Application.CQRS.Room.Commands
{
    public record UpdateRoomCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int FloorId { get; init; }
        public int RoomTypeId { get; init; }
    }
} 