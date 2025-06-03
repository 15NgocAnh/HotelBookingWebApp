using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.RoomTypeAggregate;

/// <summary>
/// Represents a type of room in the hotel.
/// This is an aggregate root entity that defines room characteristics, pricing, and amenities.
/// </summary>
public class RoomType : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// Gets the name of the room type.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the base price for this room type.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Private collection of bed type setup details for this room type.
    /// </summary>
    private readonly List<BedTypeSetupDetail> _bedTypeSetupDetails = [];

    /// <summary>
    /// Gets a read-only collection of bed type setup details for this room type.
    /// </summary>
    public IReadOnlyCollection<BedTypeSetupDetail> BedTypeSetupDetails => _bedTypeSetupDetails.AsReadOnly();

    /// <summary>
    /// Private collection of amenity setup details for this room type.
    /// </summary>
    private readonly List<AmenitySetupDetail> _amenitySetupDetails = [];

    /// <summary>
    /// Gets a read-only collection of amenity setup details for this room type.
    /// </summary>
    public IReadOnlyCollection<AmenitySetupDetail> AmenitySetupDetails => _amenitySetupDetails.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the RoomType class.
    /// Required for Entity Framework Core.
    /// </summary>
    public RoomType()
    {
    }

    /// <summary>
    /// Initializes a new instance of the RoomType class with specified details.
    /// </summary>
    /// <param name="name">The name of the room type.</param>
    /// <param name="price">The base price for this room type.</param>
    /// <param name="bedTypeSetupDetails">The collection of bed type setup details.</param>
    /// <param name="amenitySetupDetails">The collection of amenity setup details.</param>
    public RoomType(string name,
        decimal price,
        IEnumerable<BedTypeSetupDetail> bedTypeSetupDetails,
        IEnumerable<AmenitySetupDetail> amenitySetupDetails)
    {
        Name = name;
        Price = price;
        _bedTypeSetupDetails.AddRange(bedTypeSetupDetails);
        _amenitySetupDetails.AddRange(amenitySetupDetails);
    }

    /// <summary>
    /// Updates the room type's information and its associated details.
    /// </summary>
    /// <param name="name">The new name of the room type.</param>
    /// <param name="price">The new base price.</param>
    /// <param name="bedTypeSetupDetails">The new collection of bed type setup details.</param>
    /// <param name="amenitySetupDetails">The new collection of amenity setup details.</param>
    public void Update(string name,
        decimal price,
        IEnumerable<BedTypeSetupDetail> bedTypeSetupDetails,
        IEnumerable<AmenitySetupDetail> amenitySetupDetails)
    {
        Name = name;
        Price = price;
        _bedTypeSetupDetails.Clear();
        _bedTypeSetupDetails.AddRange(bedTypeSetupDetails);
        _amenitySetupDetails.Clear();
        _amenitySetupDetails.AddRange(amenitySetupDetails);
    }
}
