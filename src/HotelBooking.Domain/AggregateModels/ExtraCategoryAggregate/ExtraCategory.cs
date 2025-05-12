using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.ExtraCategoryAggregate;
public class ExtraCategory(string name) : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; } = name;

    public void Update(string name)
    {
        Name = name;
    }
}
