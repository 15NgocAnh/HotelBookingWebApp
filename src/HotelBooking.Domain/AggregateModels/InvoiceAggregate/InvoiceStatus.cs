namespace HotelBooking.Domain.AggregateModels.InvoiceAggregate
{
    public enum InvoiceStatus
    {
        Pending,
        Paid,
        PartiallyPaid,
        Overdue,
        Cancelled
    }
} 