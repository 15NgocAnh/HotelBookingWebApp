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
    public PaymentStatus PaymentStatus { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal PaidAmount { get; private set; }
    public string? SpecialRequests { get; private set; }
    public string? Notes { get; private set; }
    public bool IsLateCheckIn { get; private set; }
    public bool IsLateCheckOut { get; private set; }
    public bool HasDamageReport { get; private set; }
    public string? DamageReport { get; private set; }

    private readonly List<Guest> _guests = [];
    public IReadOnlyCollection<Guest> Guests => _guests.AsReadOnly();

    private readonly List<ExtraUsage> _extraUsages = [];
    public IReadOnlyCollection<ExtraUsage> ExtraUsages => _extraUsages.AsReadOnly();

    private readonly List<Payment> _payments = [];
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    public Booking()
    {
        
    }

    public Booking(Guest guest)
    {
        BookingTime = DateTime.UtcNow;
        Status = BookingStatus.Pending;
        PaymentStatus = PaymentStatus.Unpaid;
        _guests.Add(guest);
    }

    public Booking(int roomId, IEnumerable<Guest> guests)
    {
        RoomId = roomId;
        BookingTime = DateTime.UtcNow;
        CheckInTime = DateTime.UtcNow;
        Status = BookingStatus.CheckedIn;
        PaymentStatus = PaymentStatus.Unpaid;
        _guests.AddRange(guests);
    }

    public void Update(int roomId, DateTime? checkInTime, DateTime? checkOutTime, IEnumerable<Guest> guests, string? specialRequests = null, string? notes = null)
    {
        RoomId = roomId;
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
        SpecialRequests = specialRequests;
        Notes = notes;
        _guests.Clear();
        _guests.AddRange(guests);
        SetUpdatedAt();
    }

    public void CheckIn()
    {
        if (Status != BookingStatus.Pending)
            throw new DomainException("Booking must be in Pending status to check in");

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

    public void AddDamageReport(string report)
    {
        if (string.IsNullOrWhiteSpace(report))
            throw new DomainException("Damage report cannot be empty");

        HasDamageReport = true;
        DamageReport = report;
        SetUpdatedAt();
    }

    public void AddPayment(decimal amount, string paymentMethod, string? notes = null)
    {
        if (amount <= 0)
            throw new DomainException("Payment amount must be greater than zero");

        var payment = new Payment(amount, paymentMethod, notes);
        _payments.Add(payment);
        
        PaidAmount += amount;
        UpdatePaymentStatus();
        SetUpdatedAt();
    }

    public void AddExtraUsage(int extraItemId, string extraItemName, int quantity, decimal unitPrice)
    {
        var totalAmount = quantity * unitPrice;
        var extraUsage = new ExtraUsage(extraItemId, extraItemName, quantity, totalAmount);
        _extraUsages.Add(extraUsage);
        
        TotalAmount += totalAmount;
        UpdatePaymentStatus();
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
                if (PaymentStatus == PaymentStatus.Paid)
                    throw new DomainException("Cannot cancel a fully paid booking");
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

    private void UpdatePaymentStatus()
    {
        if (TotalAmount == 0)
        {
            PaymentStatus = PaymentStatus.Unpaid;
            return;
        }

        if (PaidAmount >= TotalAmount)
        {
            PaymentStatus = PaymentStatus.Paid;
        }
        else if (PaidAmount > 0)
        {
            PaymentStatus = PaymentStatus.PartiallyPaid;
        }
        else
        {
            PaymentStatus = PaymentStatus.Unpaid;
        }
    }
}