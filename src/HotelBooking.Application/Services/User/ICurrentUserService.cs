namespace HotelBooking.Application.Services.User
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string Role { get; }
        IEnumerable<int> UserHotelIds { get; }
    }
}
