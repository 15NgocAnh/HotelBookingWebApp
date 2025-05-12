namespace HotelBooking.Application.Common.Security;
public interface IAppUser
{
    string Id { get; }
    string? UserName { get; }
    string FirstName { get; }
    string LastName { get; }
    bool IsDisabled { get; }
}
