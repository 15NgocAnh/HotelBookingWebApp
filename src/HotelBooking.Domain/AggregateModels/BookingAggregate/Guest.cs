using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.BookingAggregate;
public class Guest(string citizenIdNumber, string passportNumber, string firstName, string lastName, string phoneNumber) : ValueObject
{
    public string CitizenIdNumber { get; private init; } = citizenIdNumber;
    public string PassportNumber { get; private init; } = passportNumber;
    public string FirstName { get; private init; } = firstName;
    public string LastName { get; private init; } = lastName;
    public string PhoneNumber { get; private init; } = phoneNumber;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CitizenIdNumber;
        yield return PassportNumber;
        yield return FirstName;
        yield return LastName;
        yield return PhoneNumber;
    }
}
