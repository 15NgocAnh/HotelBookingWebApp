namespace HotelBooking.Domain.AggregateModels.UserAggregate;

/// <summary>
/// Represents the possible status values for a user account in the system.
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// Indicates that the user account is active and can be used.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Indicates that the user account is inactive and cannot be used.
    /// </summary>
    Inactive = 2
} 