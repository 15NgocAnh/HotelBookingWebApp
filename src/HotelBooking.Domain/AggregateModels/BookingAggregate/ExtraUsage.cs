namespace HotelBooking.Domain.AggregateModels.BookingAggregate;
public class ExtraUsage(int extraItemId, string extraItemName, int quantity, decimal totalAmount) : ValueObject
{
    public int ExtraItemId { get; private init; } = extraItemId;
    public string ExtraItemName { get; private init; } = extraItemName;
    public int Quantity { get; private init; } = quantity;
    public decimal TotalAmount { get; private init; } = totalAmount;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ExtraItemId;
        yield return Quantity;
        yield return TotalAmount;
    }
}
