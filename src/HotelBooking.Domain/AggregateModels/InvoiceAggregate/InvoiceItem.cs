using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Domain.AggregateModels.InvoiceAggregate
{
    /// <summary>
    /// Represents an item in an invoice.
    /// This is a value object that contains information about a specific item being charged in the invoice,
    /// including its description, quantity, unit price, and total price.
    /// </summary>
    public class InvoiceItem : ValueObject
    {
        /// <summary>
        /// Gets the description of the item.
        /// </summary>
        public string? Description { get; private init; }

        /// <summary>
        /// Gets the quantity of the item.
        /// </summary>
        public int Quantity { get; private init; }

        /// <summary>
        /// Gets the unit price of the item.
        /// </summary>
        public decimal UnitPrice { get; private init; }

        /// <summary>
        /// Gets the total price of the item (quantity * unit price).
        /// </summary>
        public decimal TotalPrice { get; private init; }

        /// <summary>
        /// Gets the type of the item (e.g., room charge, extra service, etc.).
        /// </summary>
        public string Type { get; private init; }

        /// <summary>
        /// Initializes a new instance of the InvoiceItem class.
        /// </summary>
        /// <param name="description">The description of the item.</param>
        /// <param name="quantity">The quantity of the item.</param>
        /// <param name="unitPrice">The unit price of the item.</param>
        /// <param name="type">The type of the item.</param>
        /// <exception cref="DomainException">Thrown when any of the parameters are invalid.</exception>
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

        /// <summary>
        /// Gets the components that define equality for this value object.
        /// </summary>
        /// <returns>An enumerable of objects that define the equality of this invoice item.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
            yield return Quantity;
            yield return UnitPrice;
            yield return Type;
        }
    }
} 