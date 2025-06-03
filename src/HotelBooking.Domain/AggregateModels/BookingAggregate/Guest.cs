using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.BookingAggregate;

/// <summary>
/// Represents a guest in a booking.
/// This is a value object that contains guest identification and contact information.
/// </summary>
public class Guest(string citizenIdNumber, string passportNumber, string firstName, string lastName, string phoneNumber) : ValueObject
{
    /// <summary>
    /// Gets the guest's citizen ID number, if available.
    /// </summary>
    public string? CitizenIdNumber { get; private init; } = citizenIdNumber;

    /// <summary>
    /// Gets the guest's passport number, if available.
    /// </summary>
    public string? PassportNumber { get; private init; } = passportNumber;

    /// <summary>
    /// Gets the guest's first name.
    /// </summary>
    public string FirstName { get; private init; } = firstName;

    /// <summary>
    /// Gets the guest's last name.
    /// </summary>
    public string LastName { get; private init; } = lastName;

    /// <summary>
    /// Gets the guest's phone number, if available.
    /// </summary>
    public string? PhoneNumber { get; private init; } = phoneNumber;

    /// <summary>
    /// Gets the components that define the equality of this value object.
    /// </summary>
    /// <returns>An enumerable of objects that define the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CitizenIdNumber;
        yield return PassportNumber;
        yield return FirstName;
        yield return LastName;
        yield return PhoneNumber;
    }
}
