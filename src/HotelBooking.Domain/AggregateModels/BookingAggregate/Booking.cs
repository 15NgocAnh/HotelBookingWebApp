using HotelBooking.Domain.AggregateModels.RoomAggregate;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Domain.AggregateModels.BookingAggregate;

public class Booking : BaseEntity, IAggregateRoot
{
    public int RoomId { get; private set; }
    public DateTime CheckInDate { get; private set; }
    public DateTime CheckOutDate { get; private set; }
    public DateTime? CheckInTime { get; private set; }
    public DateTime? CheckOutTime { get; private set; }
    public BookingStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public bool IsLateCheckIn { get; private set; }
    public bool IsLateCheckOut { get; private set; }
    public bool HasDamageReport { get; private set; }
    public string? DamageReport { get; private set; }

    private readonly List<Guest> _guests = [];
    public IReadOnlyCollection<Guest> Guests => _guests.AsReadOnly();

    private readonly List<ExtraUsage> _extraUsages = [];
    public IReadOnlyCollection<ExtraUsage> ExtraUsages => _extraUsages.AsReadOnly();

    public Booking()
    {
    }

    public Booking(int roomId)
    {
        RoomId = roomId;
    }

    public void Update(int roomId, DateTime checkInDate, DateTime checkOutDate, IEnumerable<Guest> guests, string? notes = null)
    {
        RoomId = roomId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        Notes = notes;
        _guests.Clear();
        _guests.AddRange(guests);
        SetUpdatedAt();
    }

    public void CheckIn()
    {
        if (Status != BookingStatus.Confirmed)
            throw new DomainException("Booking must be in Confirmed status to check in");

        if (CheckInTime.HasValue)
            throw new DomainException("Guest has already checked in");

        CheckInTime = DateTime.UtcNow;
        Status = BookingStatus.CheckedIn;
        IsLateCheckIn = CheckInTime.Value.TimeOfDay > TimeSpan.FromHours(14); // After 2 PM is considered late check-in
        SetUpdatedAt();
    }

    public void CheckOut()
    {
        if (Status != BookingStatus.CheckedIn)
            throw new DomainException("Guest must be checked in before checking out");

        if (CheckOutTime.HasValue)
            throw new DomainException("Guest has already checked out");

        CheckOutTime = DateTime.UtcNow;
        Status = BookingStatus.CheckedOut;
        IsLateCheckOut = CheckOutTime.Value.TimeOfDay > TimeSpan.FromHours(12); // After 12 PM is considered late check-out
        SetUpdatedAt();
    }

    public bool CanGenerateInvoice()
    {
        return Status == BookingStatus.CheckedOut;
    }

    public void AddDamageReport(string report)
    {
        if (string.IsNullOrWhiteSpace(report))
            throw new DomainException("Damage report cannot be empty");

        HasDamageReport = true;
        DamageReport = report;
        SetUpdatedAt();
    }

    public void AddExtraUsage(int extraItemId, string extraItemName, int quantity, decimal unitPrice)
    {
        var totalAmount = quantity * unitPrice;
        var extraUsage = new ExtraUsage(extraItemId, extraItemName, quantity, totalAmount);
        _extraUsages.Add(extraUsage);
        
        SetUpdatedAt();
    }

    public void ClearExtraUsages()
    {
        _extraUsages.Clear();
        SetUpdatedAt();
    }

    public void Cancel(string? reason = null)
    {
        if (Status == BookingStatus.CheckedOut)
            throw new DomainException("Cannot cancel a checked out booking");

        Status = BookingStatus.Cancelled;
        Notes = reason ?? Notes;
        SetUpdatedAt();
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

            case BookingStatus.Confirmed:
                if (Status != BookingStatus.Pending)
                    throw new DomainException("Cannot set status to Confirmed unless it was Pending");
                break;

            case BookingStatus.CheckedIn:
                if (Status != BookingStatus.Pending)
                    throw new DomainException("Can only check in from Pending status");
                CheckInTime = DateTime.UtcNow;
                IsLateCheckIn = CheckInTime.Value.TimeOfDay > TimeSpan.FromHours(14);
                break;

            case BookingStatus.CheckedOut:
                if (Status != BookingStatus.CheckedIn)
                    throw new DomainException("Can only check out from CheckedIn status");
                CheckOutTime = DateTime.UtcNow;
                IsLateCheckOut = CheckOutTime.Value.TimeOfDay > TimeSpan.FromHours(12);
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
        SetUpdatedAt();
    }
}