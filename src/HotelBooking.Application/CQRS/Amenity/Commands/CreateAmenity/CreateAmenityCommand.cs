namespace HotelBooking.Application.CQRS.Amenity.Commands.CreateAmenity
{
    public record CreateAmenityCommand : ICommand<Result<int>>
    {
        public string Name { get; init; }
    }
}