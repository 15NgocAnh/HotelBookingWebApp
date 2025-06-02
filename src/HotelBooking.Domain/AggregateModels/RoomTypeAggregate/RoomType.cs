namespace HotelBooking.Domain.AggregateModels.RoomTypeAggregate;
public class RoomType : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    private readonly List<BedTypeSetupDetail> _bedTypeSetupDetails = [];
    public IReadOnlyCollection<BedTypeSetupDetail> BedTypeSetupDetails => _bedTypeSetupDetails.AsReadOnly();

    private readonly List<AmenitySetupDetail> _amenitySetupDetails = [];
    public IReadOnlyCollection<AmenitySetupDetail> AmenitySetupDetails => _amenitySetupDetails.AsReadOnly();

    public RoomType()
    {
        
    }

    public RoomType(string name,
        decimal price,
        IEnumerable<BedTypeSetupDetail> bedTypeSetupDetails,
        IEnumerable<AmenitySetupDetail> amenitySetupDetails)
    {
        Name = name;
        Price = price;
        _bedTypeSetupDetails.AddRange(bedTypeSetupDetails);
        _amenitySetupDetails.AddRange(amenitySetupDetails);
    }

    public void Update(string name,
        decimal price,
        IEnumerable<BedTypeSetupDetail> bedTypeSetupDetails,
        IEnumerable<AmenitySetupDetail> amenitySetupDetails)
    {
        Name = name;
        Price = price;
        _bedTypeSetupDetails.Clear();
        _bedTypeSetupDetails.AddRange(bedTypeSetupDetails);
        _amenitySetupDetails.Clear();
        _amenitySetupDetails.AddRange(amenitySetupDetails);
    }
}
