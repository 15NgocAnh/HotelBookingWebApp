using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.BuildingAggregate;
public class Building : BaseEntity, IAggregateRoot
{
    public int HotelId { get; private init; }
    public string Name { get; private set; }

    private readonly List<Floor> _floors = [];
    public IReadOnlyCollection<Floor> Floors => _floors.AsReadOnly();

    public Building()
    {
        
    }

    public Building(int hotelId, string name, int totalFloors)
    {
        HotelId = hotelId;
        Name = name;
        _floors.AddRange(AddFloors(totalFloors));
    }

    private IEnumerable<Floor> AddFloors(int floorCount)
    {
        var startingFloor = _floors.Count + 1;
        return Enumerable
            .Range(startingFloor, floorCount)
            .Select(f => new Floor(f, $"Floor {f}"));
    }

    public void Update(string name, int totalFloors)
    {
        Name = name;
        if (totalFloors > _floors.Count)
        {
            var newFloors = AddFloors(totalFloors - _floors.Count);
            _floors.AddRange(newFloors);
        }
        else if (totalFloors < _floors.Count)
        {
            var floorsToRemove = _floors.Skip(totalFloors).ToList();
            foreach (var floor in floorsToRemove)
            {
                _floors.Remove(floor);
            }
        }
    }
}
