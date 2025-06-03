using HotelBooking.Domain.Utils.Enum;

namespace HotelBooking.Domain.AggregateModels.InvoiceAggregate
{
    /// <summary>
    /// Represents a payment made for an invoice.
    /// This is a value object that contains payment information including amount, timestamp, and payment method.
    /// </summary>
    public class Payment(decimal Amount, PaymentMethod PaymentMethod) : ValueObject
    {
        /// <summary>
        /// Gets the amount of the payment.
        /// </summary>
        public decimal Amount { get; private set; } = Amount;

        /// <summary>
        /// Gets the date and time when the payment was made.
        /// </summary>
        public DateTime PaidAt { get; private set; } = DateTime.Now;

        /// <summary>
        /// Gets the method used for the payment.
        /// </summary>
        public PaymentMethod PaymentMethod { get; private set; } = PaymentMethod;

        /// <summary>
        /// Gets the components that define equality for this value object.
        /// </summary>
        /// <returns>An enumerable of objects that define the equality of this payment.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return PaidAt;
            yield return PaymentMethod;
        }
    }
}
