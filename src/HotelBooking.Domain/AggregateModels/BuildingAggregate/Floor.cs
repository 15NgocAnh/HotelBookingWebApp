namespace HotelBooking.Domain.AggregateModels.BuildingAggregate;
public class Floor(int number, string name) : BaseEntity
{
    public int Number { get; private init; } = number;
    public string Name { get; private set; } = name;

    public void Update(string name)
    {
        Name = name;
    }
}
