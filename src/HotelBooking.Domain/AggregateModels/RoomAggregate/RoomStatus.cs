namespace HotelBooking.Domain.AggregateModels.RoomAggregate;

/// <summary>
/// Represents the possible status values for a room in the hotel.
/// </summary>
public enum RoomStatus
{
    /// <summary>
    /// Indicates that the room is available for booking.
    /// </summary>
    Available,

    /// <summary>
    /// Indicates that the room is currently booked by a guest.
    /// </summary>
    Booked,

    /// <summary>
    /// Indicates that the room is being cleaned after a guest's stay.
    /// </summary>
    CleaningUp,

    /// <summary>
    /// Indicates that the room is undergoing maintenance and is not available for booking.
    /// </summary>
    UnderMaintenance
}
