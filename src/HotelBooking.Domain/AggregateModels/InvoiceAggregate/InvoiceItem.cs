using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Domain.AggregateModels.InvoiceAggregate
{
    public class InvoiceItem : ValueObject
    {
        public string? Description { get; private init; }
        public int Quantity { get; private init; }
        public decimal UnitPrice { get; private init; }
        public decimal TotalPrice { get; private init; }
        public string Type { get; private init; }

        public InvoiceItem(string description, int quantity, decimal unitPrice, string type)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description cannot be empty");

            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");

            if (unitPrice < 0)
                throw new DomainException("Unit price cannot be negative");

            if (string.IsNullOrWhiteSpace(type))
                throw new DomainException("Type cannot be empty");

            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = quantity * unitPrice;
            Type = type;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
            yield return Quantity;
            yield return UnitPrice;
            yield return Type;
        }
    }
} 