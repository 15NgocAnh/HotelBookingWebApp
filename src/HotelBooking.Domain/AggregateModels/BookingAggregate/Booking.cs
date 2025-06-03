using HotelBooking.Domain.AggregateModels.RoomAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Domain.AggregateModels.BookingAggregate;

/// <summary>
/// Represents a booking in the hotel system.
/// This is an aggregate root entity that manages the booking lifecycle and associated data.
/// </summary>
public class Booking : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// Gets the ID of the room being booked.
    /// </summary>
    public int RoomId { get; private set; }

    /// <summary>
    /// Gets the scheduled check-in date.
    /// </summary>
    public DateTime CheckInDate { get; private set; }

    /// <summary>
    /// Gets the scheduled check-out date.
    /// </summary>
    public DateTime CheckOutDate { get; private set; }

    /// <summary>
    /// Gets the actual check-in time, if the guest has checked in.
    /// </summary>
    public DateTime? CheckInTime { get; private set; }

    /// <summary>
    /// Gets the actual check-out time, if the guest has checked out.
    /// </summary>
    public DateTime? CheckOutTime { get; private set; }

    /// <summary>
    /// Gets the current status of the booking.
    /// </summary>
    public BookingStatus Status { get; private set; }

    /// <summary>
    /// Gets any additional notes for the booking.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the check-in was late.
    /// </summary>
    public bool IsLateCheckIn { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the check-out was late.
    /// </summary>
    public bool IsLateCheckOut { get; private set; }

    /// <summary>
    /// Gets a value indicating whether there is a damage report for this booking.
    /// </summary>
    public bool HasDamageReport { get; private set; }

    /// <summary>
    /// Gets the damage report details, if any.
    /// </summary>
    public string? DamageReport { get; private set; }

    /// <summary>
    /// Private collection of guests associated with this booking.
    /// </summary>
    private readonly List<Guest> _guests = [];

    /// <summary>
    /// Gets a read-only collection of guests associated with this booking.
    /// </summary>
    public IReadOnlyCollection<Guest> Guests => _guests.AsReadOnly();

    /// <summary>
    /// Private collection of extra items used during this booking.
    /// </summary>
    private readonly List<ExtraUsage> _extraUsages = [];

    /// <summary>
    /// Gets a read-only collection of extra items used during this booking.
    /// </summary>
    public IReadOnlyCollection<ExtraUsage> ExtraUsages => _extraUsages.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the Booking class.
    /// Required for Entity Framework Core.
    /// </summary>
    public Booking()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Booking class with a specified room.
    /// </summary>
    /// <param name="roomId">The ID of the room being booked.</param>
    public Booking(int roomId)
    {
        RoomId = roomId;
    }

    /// <summary>
    /// Updates the booking's basic information.
    /// </summary>
    /// <param name="roomId">The new room ID.</param>
    /// <param name="checkInDate">The new check-in date.</param>
    /// <param name="checkOutDate">The new check-out date.</param>
    /// <param name="guests">The new collection of guests.</param>
    /// <param name="notes">Optional notes for the booking.</param>
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

    /// <summary>
    /// Processes the check-in of the guest.
    /// </summary>
    /// <exception cref="DomainException">Thrown when the booking is not in Confirmed status or has already been checked in.</exception>
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

    /// <summary>
    /// Processes the check-out of the guest.
    /// </summary>
    /// <exception cref="DomainException">Thrown when the guest is not checked in or has already checked out.</exception>
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

    /// <summary>
    /// Determines whether an invoice can be generated for this booking.
    /// </summary>
    /// <returns>True if the booking is checked out and an invoice can be generated; otherwise, false.</returns>
    public bool CanGenerateInvoice()
    {
        return Status == BookingStatus.CheckedOut;
    }

    /// <summary>
    /// Adds a damage report to the booking.
    /// </summary>
    /// <param name="report">The details of the damage report.</param>
    /// <exception cref="DomainException">Thrown when the damage report is empty.</exception>
    public void AddDamageReport(string report)
    {
        if (string.IsNullOrWhiteSpace(report))
            throw new DomainException("Damage report cannot be empty");

        HasDamageReport = true;
        DamageReport = report;
        SetUpdatedAt();
    }

    /// <summary>
    /// Adds an extra item usage to the booking.
    /// </summary>
    /// <param name="extraItemId">The ID of the extra item.</param>
    /// <param name="extraItemName">The name of the extra item.</param>
    /// <param name="quantity">The quantity used.</param>
    /// <param name="unitPrice">The unit price of the item.</param>
    public void AddExtraUsage(int extraItemId, string extraItemName, int quantity, decimal unitPrice)
    {
        var totalAmount = quantity * unitPrice;
        var extraUsage = new ExtraUsage(extraItemId, extraItemName, quantity, totalAmount);
        _extraUsages.Add(extraUsage);
        
        SetUpdatedAt();
    }

    /// <summary>
    /// Removes all extra item usages from the booking.
    /// </summary>
    public void ClearExtraUsages()
    {
        _extraUsages.Clear();
        SetUpdatedAt();
    }

    /// <summary>
    /// Cancels the booking.
    /// </summary>
    /// <param name="reason">Optional reason for cancellation.</param>
    /// <exception cref="DomainException">Thrown when attempting to cancel a checked out booking.</exception>
    public void Cancel(string? reason = null)
    {
        if (Status == BookingStatus.CheckedOut)
            throw new DomainException("Cannot cancel a checked out booking");

        Status = BookingStatus.Cancelled;
        Notes = reason ?? Notes;
        SetUpdatedAt();
    }

    /// <summary>
    /// Updates the status of the booking.
    /// </summary>
    /// <param name="newStatus">The new status to set.</param>
    /// <exception cref="DomainException">Thrown when the status transition is invalid or the booking is already in the requested status.</exception>
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