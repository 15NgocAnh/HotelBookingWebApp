using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Domain.AggregateModels.InvoiceAggregate;

public class PaymentRecord : ValueObject
{
    public decimal Amount { get; private init; }
    public string PaymentMethod { get; private init; }
    public string? Notes { get; private init; }
    public DateTime PaymentTime { get; private init; }

    public PaymentRecord(decimal amount, string paymentMethod, string? notes = null)
    {
        if (amount <= 0)
            throw new DomainException("Payment amount must be greater than zero");

        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw new DomainException("Payment method is required");

        Amount = amount;
        PaymentMethod = paymentMethod;
        Notes = notes;
        PaymentTime = DateTime.UtcNow;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return PaymentMethod;
        yield return PaymentTime;
    }
} 