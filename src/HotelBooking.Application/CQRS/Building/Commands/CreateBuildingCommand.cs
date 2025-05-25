namespace HotelBooking.Application.CQRS.Building.Commands
{
    public record CreateBuildingCommand : ICommand<Result<int>>
    {
        public int HotelId { get; set; }
        public string Name { get; init; }
        public int TotalFloors { get; init; }
    }
} 