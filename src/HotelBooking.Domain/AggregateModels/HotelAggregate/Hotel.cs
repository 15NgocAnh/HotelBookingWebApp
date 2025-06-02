using HotelBooking.Domain.AggregateModels.BuildingAggregate;

namespace HotelBooking.Domain.AggregateModels.HotelAggregate;
public class Hotel : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string Address { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Website { get; private set; }

    private readonly List<Building> _buildings = [];
    public IReadOnlyCollection<Building> Buildings => _buildings.AsReadOnly();

    private Hotel() { } // For EF

    public Hotel(string name, string? description, string address, string phone, string? email, string? website)
    {
        Name = name;
        Description = description;
        Address = address;
        Phone = phone;
        Email = email;
        Website = website;
    }

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

    public void AddBuilding(Building building)
    {
        if (!_buildings.Any(b => b.Id == building.Id))
        {
            _buildings.Add(building);
        }
    }

    public void RemoveBuilding(Building building)
    {
        var existingBuilding = _buildings.FirstOrDefault(b => b.Id == building.Id);
        if (existingBuilding != null)
        {
            _buildings.Remove(existingBuilding);
        }
    }
}
