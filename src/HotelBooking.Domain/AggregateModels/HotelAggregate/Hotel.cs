using HotelBooking.Domain.AggregateModels.BuildingAggregate;
using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.HotelAggregate;

/// <summary>
/// Represents a hotel in the booking system.
/// This is an aggregate root entity that manages hotel information and its associated buildings.
/// </summary>
public class Hotel : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// Gets the name of the hotel.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the optional description of the hotel.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets the physical address of the hotel.
    /// </summary>
    public string Address { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the contact phone number of the hotel.
    /// </summary>
    public string Phone { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the optional contact email of the hotel.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Gets the optional website URL of the hotel.
    /// </summary>
    public string? Website { get; private set; }

    /// <summary>
    /// Private collection of buildings associated with this hotel.
    /// </summary>
    private readonly List<Building> _buildings = [];

    /// <summary>
    /// Gets a read-only collection of buildings associated with this hotel.
    /// </summary>
    public IReadOnlyCollection<Building> Buildings => _buildings.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the Hotel class.
    /// Required for Entity Framework Core.
    /// </summary>
    private Hotel() { }

    /// <summary>
    /// Initializes a new instance of the Hotel class with specified details.
    /// </summary>
    /// <param name="name">The name of the hotel.</param>
    /// <param name="description">The optional description of the hotel.</param>
    /// <param name="address">The physical address of the hotel.</param>
    /// <param name="phone">The contact phone number.</param>
    /// <param name="email">The optional contact email.</param>
    /// <param name="website">The optional website URL.</param>
    public Hotel(string name, string? description, string address, string phone, string? email, string? website)
    {
        Name = name;
        Description = description;
        Address = address;
        Phone = phone;
        Email = email;
        Website = website;
    }

    /// <summary>
    /// Updates the hotel's information.
    /// </summary>
    /// <param name="name">The new name of the hotel.</param>
    /// <param name="description">The new description of the hotel.</param>
    /// <param name="address">The new physical address.</param>
    /// <param name="phone">The new contact phone number.</param>
    /// <param name="email">The new contact email.</param>
    /// <param name="website">The new website URL.</param>
    public void Update(string name, string? description, string address, string phone, string? email, string? website)
    {
        Name = name;
        Description = description;
        Address = address;
        Phone = phone;
        Email = email;
        Website = website;
        SetUpdatedAt();
    }

    /// <summary>
    /// Adds a building to the hotel if it doesn't already exist.
    /// </summary>
    /// <param name="building">The building to add.</param>
    public void AddBuilding(Building building)
    {
        if (!_buildings.Any(b => b.Id == building.Id))
        {
            _buildings.Add(building);
        }
    }

    /// <summary>
    /// Removes a building from the hotel if it exists.
    /// </summary>
    /// <param name="building">The building to remove.</param>
    public void RemoveBuilding(Building building)
    {
        var existingBuilding = _buildings.FirstOrDefault(b => b.Id == building.Id);
        if (existingBuilding != null)
        {
            _buildings.Remove(existingBuilding);
        }
    }
}
