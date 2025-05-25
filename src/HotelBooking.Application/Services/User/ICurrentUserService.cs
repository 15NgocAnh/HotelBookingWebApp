namespace HotelBooking.Application.Services.User
{
    public interface ICurrentUserService
    {
        IEnumerable<int> UserHotelIds { get; }
    }
}
