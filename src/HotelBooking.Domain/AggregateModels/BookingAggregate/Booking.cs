using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Domain.AggregateModels.BookingAggregate;

public class Booking : BaseEntity, IAggregateRoot
{
    public int RoomId { get; private set; }
    public DateTime BookingTime { get; private init; }
    public DateTime? CheckInTime { get; private set; }
    public DateTime? CheckOutTime { get; private set; }
    public BookingStatus Status { get; private set; }

    private readonly List<Guest> _guest = [];
    public IReadOnlyCollection<Guest> Guests => _guest.AsReadOnly();

    private readonly List<ExtraUsage> _extraUsages = [];
    public IReadOnlyCollection<ExtraUsage> ExtraUsages => _extraUsages.AsReadOnly();

    public Booking()
    {
        
    }

    public Booking(Guest guest)
    {
        BookingTime = DateTime.UtcNow;
        Status = BookingStatus.Pending;
        _guest.Add(guest);
    }

    public Booking(int roomId, IEnumerable<Guest> guests)
    {
        RoomId = roomId;
        BookingTime = DateTime.UtcNow;
        CheckInTime = DateTime.UtcNow;
        Status = BookingStatus.CheckedIn;
        _guest.AddRange(guests);
    }

    public void Update(int roomId, DateTime? checkInTime, DateTime? checkOutTime, IEnumerable<Guest> guests)
    {
        RoomId = roomId;
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
        _guest.Clear();
        _guest.AddRange(guests);
    }

    public void CheckIn()
    {
        if (Status != BookingStatus.Pending)
            throw new DomainException("Booking must be in Pending status to check in");

        if (CheckInTime.HasValue)
            throw new DomainException("Guest has already checked in");

        CheckInTime = DateTime.UtcNow;
        Status = BookingStatus.CheckedIn;
    }

    public void CheckOut()
    {
        if (Status != BookingStatus.CheckedIn)
            throw new DomainException("Guest must be checked in before checking out");

        if (CheckOutTime.HasValue)
            throw new DomainException("Guest has already checked out");

        CheckOutTime = DateTime.UtcNow;
        Status = BookingStatus.CheckedOut;
    }

    public void UpdateStatus(BookingStatus newStatus)
    {
        if (Status == newStatus)
            throw new DomainException("Booking is already in the requested status");

        switch (newStatus)
        {
            case BookingStatus.Pending:
                if (Status != BookingStatus.Cancelled)
                    throw new DomainException("Cannot set status to Pending unless it was Cancelled");
                break;

            case BookingStatus.CheckedIn:
                if (Status != BookingStatus.Pending)
                    throw new DomainException("Can only check in from Pending status");
                CheckInTime = DateTime.UtcNow;
                break;

            case BookingStatus.CheckedOut:
                if (Status != BookingStatus.CheckedIn)
                    throw new DomainException("Can only check out from CheckedIn status");
                CheckOutTime = DateTime.UtcNow;
                break;

            case BookingStatus.Cancelled:
                if (Status == BookingStatus.CheckedOut)
                    throw new DomainException("Cannot cancel a checked out booking");
                break;

            case BookingStatus.NoShow:
                if (Status != BookingStatus.Pending)
                    throw new DomainException("Can only mark as NoShow from Pending status");
                break;

            default:
                throw new DomainException("Invalid booking status");
        }

        Status = newStatus;
    }
}