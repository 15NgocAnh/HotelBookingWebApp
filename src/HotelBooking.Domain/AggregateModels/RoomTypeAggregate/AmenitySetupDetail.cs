using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.RoomTypeAggregate;

/// <summary>
/// Represents the setup details of an amenity in a room type.
/// This is a value object that defines the quantity of a specific amenity in a room type.
/// </summary>
public class AmenitySetupDetail(int amenityId, string amenityName, int quantity) : ValueObject
{
    /// <summary>
    /// Gets the ID of the amenity.
    /// </summary>
    public int AmenityId { get; private init; } = amenityId;

    /// <summary>
    /// Gets the name of the amenity.
    /// </summary>
    public string AmenityName { get; private init; } = amenityName;

    /// <summary>
    /// Gets the quantity of this amenity in the room type.
    /// </summary>
    public int Quantity { get; private init; } = quantity;

    /// <summary>
    /// Gets the components that define the equality of this value object.
    /// </summary>
    /// <returns>An enumerable of objects that define the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return AmenityId;
        yield return Quantity;
    }
}
