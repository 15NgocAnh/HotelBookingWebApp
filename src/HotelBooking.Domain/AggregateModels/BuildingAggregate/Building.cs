using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.BuildingAggregate;

/// <summary>
/// Represents a building within a hotel.
/// This is an aggregate root entity that manages building information and its associated floors.
/// </summary>
public class Building : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// Gets the ID of the hotel this building belongs to.
    /// </summary>
    public int HotelId { get; private init; }

    /// <summary>
    /// Gets the name of the building.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Private collection of floors in this building.
    /// </summary>
    private readonly List<Floor> _floors = [];

    /// <summary>
    /// Gets a read-only collection of floors in this building.
    /// </summary>
    public IReadOnlyCollection<Floor> Floors => _floors.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the Building class.
    /// Required for Entity Framework Core.
    /// </summary>
    public Building()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Building class with specified details.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel this building belongs to.</param>
    /// <param name="name">The name of the building.</param>
    /// <param name="totalFloors">The total number of floors to create in the building.</param>
    public Building(int hotelId, string name, int totalFloors)
    {
        HotelId = hotelId;
        Name = name;
        _floors.AddRange(AddFloors(totalFloors));
    }

    /// <summary>
    /// Creates a collection of floor entities based on the specified count.
    /// </summary>
    /// <param name="floorCount">The number of floors to create.</param>
    /// <returns>A collection of newly created floor entities.</returns>
    private IEnumerable<Floor> AddFloors(int floorCount)
    {
        var startingFloor = _floors.Count + 1;
        return Enumerable
            .Range(startingFloor, floorCount)
            .Select(f => new Floor(f, $"Floor {f}"));
    }

    /// <summary>
    /// Updates the building's information and manages its floors.
    /// </summary>
    /// <param name="name">The new name of the building.</param>
    /// <param name="totalFloors">The new total number of floors.</param>
    public void Update(string name, int totalFloors)
    {
        Name = name;
        
        // If total floors is less than current floors, remove excess floors
        if (totalFloors < _floors.Count)
        {
            _floors.RemoveRange(totalFloors, _floors.Count - totalFloors);
        }
        // If total floors is more than current floors, add new floors
        else if (totalFloors > _floors.Count)
        {
            var newFloors = AddFloors(totalFloors - _floors.Count);
            _floors.AddRange(newFloors);
        }
        
        // Update floor names to ensure consistency
        for (int i = 0; i < _floors.Count; i++)
        {
            _floors[i].Update($"Floor {i + 1}");
        }
    }
}
