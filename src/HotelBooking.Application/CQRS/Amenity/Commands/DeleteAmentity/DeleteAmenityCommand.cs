namespace HotelBooking.Application.CQRS.Amenity.Commands.DeleteAmentity
{
    public record DeleteAmenityCommand(int Id) : ICommand<Result>;
}