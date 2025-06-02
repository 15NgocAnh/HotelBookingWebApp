namespace HotelBooking.Domain.AggregateModels.BookingAggregate;
public enum BookingStatus
{
    Pending,
    Confirmed,
    CheckedIn,
    CheckedOut,
    Cancelled,
    NoShow,
    Completed
}
