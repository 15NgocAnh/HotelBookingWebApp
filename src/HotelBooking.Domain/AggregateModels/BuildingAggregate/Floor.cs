namespace HotelBooking.Domain.AggregateModels.BuildingAggregate;
public class Floor : BaseEntity
{
    public int Number { get; private init; }
    public string Name { get; private set; }

    public Floor(int number, string name)
    {
        Number = number;
        Name = name;
    }

    public Floor(int id, int number, string name)
    {
        Id = id;
        Number = number;
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}
