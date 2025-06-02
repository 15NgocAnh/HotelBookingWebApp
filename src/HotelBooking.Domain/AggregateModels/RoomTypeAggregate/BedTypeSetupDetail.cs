namespace HotelBooking.Domain.AggregateModels.RoomTypeAggregate;
public class BedTypeSetupDetail(int bedTypeId, string bedTypeName, int quantity) : ValueObject
{
    public int BedTypeId { get; private init; } = bedTypeId;
    public string BedTypeName { get; private init; } = bedTypeName;
    public int Quantity { get; private init; } = quantity;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BedTypeId;
        yield return Quantity;
    }
}
