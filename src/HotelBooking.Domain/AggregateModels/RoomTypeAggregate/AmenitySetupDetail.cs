namespace HotelBooking.Domain.AggregateModels.RoomTypeAggregate;
public class AmenitySetupDetail(int amenityId, string amenityName, int quantity) : ValueObject
{
    public int AmenityId { get; private init; } = amenityId;
    public string AmenityName { get; private init; } = amenityName;
    public int Quantity { get; private init; } = quantity;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return AmenityId;
        yield return Quantity;
    }
}
