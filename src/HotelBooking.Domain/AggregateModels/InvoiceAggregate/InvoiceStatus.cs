namespace HotelBooking.Domain.AggregateModels.InvoiceAggregate
{
    /// <summary>
    /// Represents the possible status values for an invoice in the system.
    /// </summary>
    public enum InvoiceStatus
    {
        /// <summary>
        /// Indicates that the invoice is pending payment.
        /// </summary>
        Pending,

        /// <summary>
        /// Indicates that the invoice has been fully paid.
        /// </summary>
        Paid,

        /// <summary>
        /// Indicates that the invoice has been partially paid.
        /// </summary>
        PartiallyPaid,

        /// <summary>
        /// Indicates that the invoice is past its due date and payment is overdue.
        /// </summary>
        Overdue,

        /// <summary>
        /// Indicates that the invoice has been cancelled.
        /// </summary>
        Cancelled
    }
} 