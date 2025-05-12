using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.AmenityAggregate;

public class Amenity(string name) : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; } = name;

    public void Update(string name)
    {
        Name = name;
    }
}