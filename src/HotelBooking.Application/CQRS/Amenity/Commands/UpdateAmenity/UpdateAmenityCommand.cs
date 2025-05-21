namespace HotelBooking.Application.CQRS.Amenity.Commands.UpdateAmenity
{
    public record UpdateAmenityCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}