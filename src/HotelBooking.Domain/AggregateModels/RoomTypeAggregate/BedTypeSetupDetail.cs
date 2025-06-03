using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.RoomTypeAggregate;

/// <summary>
/// Represents the setup details of a bed type in a room type.
/// This is a value object that defines the quantity of a specific bed type in a room type.
/// </summary>
public class BedTypeSetupDetail(int bedTypeId, string bedTypeName, int quantity) : ValueObject
{
    /// <summary>
    /// Gets the ID of the bed type.
    /// </summary>
    public int BedTypeId { get; private init; } = bedTypeId;

    /// <summary>
    /// Gets the name of the bed type.
    /// </summary>
    public string BedTypeName { get; private init; } = bedTypeName;

    /// <summary>
    /// Gets the quantity of this bed type in the room type.
    /// </summary>
    public int Quantity { get; private init; } = quantity;

    /// <summary>
    /// Gets the components that define the equality of this value object.
    /// </summary>
    /// <returns>An enumerable of objects that define the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BedTypeId;
        yield return Quantity;
    }
}
