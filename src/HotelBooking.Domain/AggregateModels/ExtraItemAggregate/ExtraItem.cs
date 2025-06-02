using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.ExtraItemAggregate;
public class ExtraItem(int extraCategoryId, string name, decimal price) : BaseEntity, IAggregateRoot
{
    public int ExtraCategoryId { get; private set; } = extraCategoryId;
    public string Name { get; private set; } = name;
    public decimal Price { get; private set; } = price;

    public void Update(int extraCategoryId, string name, decimal price)
    {
        ExtraCategoryId = extraCategoryId;
        Name = name;
        Price = price;
    }
}
