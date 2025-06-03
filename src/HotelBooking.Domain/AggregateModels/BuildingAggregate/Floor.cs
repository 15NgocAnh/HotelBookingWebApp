using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.BuildingAggregate;

/// <summary>
/// Represents a floor within a building.
/// This is an entity that contains floor information and is part of the Building aggregate.
/// </summary>
public class Floor : BaseEntity
{
    /// <summary>
    /// Gets the floor number within the building.
    /// </summary>
    public int Number { get; private init; }

    /// <summary>
    /// Gets the name of the floor.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Floor class.
    /// </summary>
    /// <param name="number">The floor number.</param>
    /// <param name="name">The name of the floor.</param>
    public Floor(int number, string name)
    {
        Number = number;
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the Floor class with a specified ID.
    /// </summary>
    /// <param name="id">The unique identifier for the floor.</param>
    /// <param name="number">The floor number.</param>
    /// <param name="name">The name of the floor.</param>
    public Floor(int id, int number, string name)
    {
        Id = id;
        Number = number;
        Name = name;
    }

    /// <summary>
    /// Updates the name of the floor.
    /// </summary>
    /// <param name="name">The new name for the floor.</param>
    public void Update(string name)
    {
        Name = name;
    }
}
