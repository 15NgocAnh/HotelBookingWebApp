using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Domain.Utils.Enum;

namespace HotelBooking.Domain.AggregateModels.InvoiceAggregate
{
    /// <summary>
    /// Represents an invoice in the hotel system.
    /// This is an aggregate root entity that manages invoice information, payments, and status.
    /// </summary>
    public class Invoice : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// Gets the ID of the associated booking.
        /// </summary>
        public int BookingId { get; private set; }

        /// <summary>
        /// Gets the unique invoice number.
        /// </summary>
        public string InvoiceNumber { get; private set; }

        /// <summary>
        /// Gets the total amount of the invoice.
        /// </summary>
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Gets the amount that has been paid.
        /// </summary>
        public decimal PaidAmount { get; private set; }

        /// <summary>
        /// Gets the remaining amount to be paid.
        /// </summary>
        public decimal RemainingAmount { get; private set; }

        /// <summary>
        /// Gets the current status of the invoice.
        /// </summary>
        public InvoiceStatus Status { get; private set; }

        /// <summary>
        /// Gets the due date for payment.
        /// </summary>
        public DateTime DueDate { get; private set; }

        /// <summary>
        /// Gets the date when the invoice was paid, if applicable.
        /// </summary>
        public DateTime? PaidDate { get; private set; }

        /// <summary>
        /// Gets any additional notes for the invoice.
        /// </summary>
        public string? Notes { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the payment is late.
        /// </summary>
        public bool IsLatePayment { get; private set; }

        /// <summary>
        /// Gets the late payment fee amount.
        /// </summary>
        public decimal LatePaymentFee { get; private set; }

        /// <summary>
        /// Gets the reason for cancellation, if applicable.
        /// </summary>
        public string? CancellationReason { get; private set; }

        /// <summary>
        /// Gets the date when the invoice was cancelled, if applicable.
        /// </summary>
        public DateTime? CancellationDate { get; private set; }

        /// <summary>
        /// Private collection of invoice items.
        /// </summary>
        private readonly List<InvoiceItem> _items = [];

        /// <summary>
        /// Gets a read-only collection of invoice items.
        /// </summary>
        public IReadOnlyCollection<InvoiceItem> Items => _items.AsReadOnly();

        /// <summary>
        /// Private collection of payments made for this invoice.
        /// </summary>
        private readonly List<Payment> _payments = new();

        /// <summary>
        /// Gets a read-only collection of payments made for this invoice.
        /// </summary>
        public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the Invoice class.
        /// Required for Entity Framework Core.
        /// </summary>
        private Invoice() { }

        /// <summary>
        /// Initializes a new instance of the Invoice class with specified details.
        /// </summary>
        /// <param name="bookingId">The ID of the associated booking.</param>
        /// <param name="dueDate">The due date for payment.</param>
        /// <param name="notes">Optional notes for the invoice.</param>
        public Invoice(int bookingId, DateTime dueDate, string? notes)
        {
            BookingId = bookingId;
            DueDate = dueDate;
            Notes = notes;
            Status = InvoiceStatus.Pending;
            PaidAmount = 0;
            RemainingAmount = 0;
            LatePaymentFee = 0;
        }

        /// <summary>
        /// Updates the invoice's basic information.
        /// </summary>
        /// <param name="dueDate">The new due date.</param>
        /// <param name="paymentMethod">The payment method.</param>
        /// <param name="notes">The new notes.</param>
        public void Update(DateTime dueDate, string paymentMethod, string notes)
        {
            DueDate = dueDate;
            Notes = notes;
        }

        /// <summary>
        /// Adds an item to the invoice.
        /// </summary>
        /// <param name="description">The description of the item.</param>
        /// <param name="quantity">The quantity of the item.</param>
        /// <param name="unitPrice">The unit price of the item.</param>
        /// <param name="type">The type of the item.</param>
        public void AddItem(string description, int quantity, decimal unitPrice, string type)
        {
            var item = new InvoiceItem(description, quantity, unitPrice, type);
            _items.Add(item);
            CalculateAmount();
        }

        /// <summary>
        /// Adds multiple items to the invoice.
        /// </summary>
        /// <param name="invoiceItems">The collection of items to add.</param>
        public void AddRangeItem(List<InvoiceItem> invoiceItems)
        {
            _items.Clear();
            _items.AddRange(invoiceItems);
            CalculateAmount();
        }

        /// <summary>
        /// Adds a payment to the invoice.
        /// </summary>
        /// <param name="amount">The payment amount.</param>
        /// <param name="paymentMethod">The method of payment.</param>
        public void AddPayment(decimal amount, PaymentMethod paymentMethod)
        {
            var item = new Payment(amount, paymentMethod);
            _payments.Add(item);
            CalculateAmount();
        }

        /// <summary>
        /// Adds multiple payments to the invoice.
        /// </summary>
        /// <param name="payments">The collection of payments to add.</param>
        public void AddRangePayment(List<Payment> payments)
        {
            _payments.Clear();
            _payments.AddRange(payments);
            CalculateAmount();
        }

        /// <summary>
        /// Updates the status of the invoice.
        /// </summary>
        /// <param name="newStatus">The new status to set.</param>
        /// <exception cref="DomainException">Thrown when the status transition is invalid or the invoice is already in the requested status.</exception>
        public void UpdateStatus(InvoiceStatus newStatus)
        {
            if (Status == newStatus)
                throw new DomainException("Invoice is already in the requested status");

            switch (newStatus)
            {
                case InvoiceStatus.Pending:
                    if (Status != InvoiceStatus.Cancelled)
                        throw new DomainException("Cannot set status to Pending unless it was Cancelled");
                    break;

                case InvoiceStatus.Paid:
                    if (Status == InvoiceStatus.Cancelled)
                        throw new DomainException("Cannot mark a cancelled invoice as Paid");
                    if (PaidAmount < TotalAmount)
                        throw new DomainException("Cannot mark as Paid with partial payment");
                    PaidDate = DateTime.UtcNow;
                    break;

                case InvoiceStatus.PartiallyPaid:
                    if (Status == InvoiceStatus.Cancelled)
                        throw new DomainException("Cannot mark a cancelled invoice as PartiallyPaid");
                    if (PaidAmount >= TotalAmount)
                        throw new DomainException("Cannot mark as PartiallyPaid with full payment");
                    break;

                case InvoiceStatus.Overdue:
                    if (Status == InvoiceStatus.Cancelled)
                        throw new DomainException("Cannot mark a cancelled invoice as Overdue");
                    if (Status == InvoiceStatus.Paid)
                        throw new DomainException("Cannot mark a paid invoice as Overdue");
                    IsLatePayment = true;
                    CalculateLatePaymentFee();
                    break;

                case InvoiceStatus.Cancelled:
                    if (Status == InvoiceStatus.Paid)
                        throw new DomainException("Cannot cancel a paid invoice");
                    CancellationDate = DateTime.UtcNow;
                    break;

                default:
                    throw new DomainException("Invalid invoice status");
            }

            Status = newStatus;
            SetUpdatedAt();
        }

        /// <summary>
        /// Cancels the invoice.
        /// </summary>
        /// <param name="reason">The reason for cancellation.</param>
        /// <exception cref="DomainException">Thrown when attempting to cancel a paid invoice or when the reason is empty.</exception>
        public void Cancel(string reason)
        {
            if (Status == InvoiceStatus.Paid)
                throw new DomainException("Cannot cancel a paid invoice");

            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Cancellation reason is required");

            Status = InvoiceStatus.Cancelled;
            CancellationReason = reason;
            CancellationDate = DateTime.UtcNow;
            SetUpdatedAt();
        }

        /// <summary>
        /// Calculates the total amount, paid amount, and remaining amount.
        /// </summary>
        private void CalculateAmount()
        {
            TotalAmount = _items.Sum(item => item.TotalPrice);
            PaidAmount = _payments.Sum(p => p.Amount);
            RemainingAmount = TotalAmount - PaidAmount;
            UpdateStatus();
        }

        /// <summary>
        /// Updates the status of the invoice based on payment status and due date.
        /// </summary>
        private void UpdateStatus()
        {
            if (Status == InvoiceStatus.Cancelled)
                return;

            if (PaidAmount >= TotalAmount)
            {
                Status = InvoiceStatus.Paid;
                PaidDate = DateTime.UtcNow;
            }
            else if (PaidAmount > 0)
            {
                Status = InvoiceStatus.PartiallyPaid;
            }
            else if (DateTime.UtcNow > DueDate)
            {
                Status = InvoiceStatus.Overdue;
                IsLatePayment = true;
                CalculateLatePaymentFee();
            }
            else
            {
                Status = InvoiceStatus.Pending;
            }
        }

        /// <summary>
        /// Calculates the late payment fee based on the number of days late.
        /// </summary>
        private void CalculateLatePaymentFee()
        {
            if (!IsLatePayment)
                return;

            var daysLate = (DateTime.UtcNow - DueDate).Days;
            LatePaymentFee = Math.Round(RemainingAmount * 0.01m * daysLate, 2); // 1% per day late
        }

        /// <summary>
        /// Calculates the total amount including room price and items.
        /// </summary>
        /// <param name="roomPrice">The price of the room.</param>
        public void CalculateTotalAmount(decimal roomPrice)
        {
            TotalAmount += roomPrice;
            foreach (var item in _items)
            {
                TotalAmount += item.TotalPrice;
            }
            CalculateRemainingAmount();
        }

        /// <summary>
        /// Calculates the remaining amount to be paid.
        /// </summary>
        public void CalculateRemainingAmount()
        {
            RemainingAmount = TotalAmount - PaidAmount;
        }

        /// <summary>
        /// Sets the invoice number.
        /// </summary>
        /// <param name="invoiceNumber">The invoice number to set.</param>
        /// <exception cref="DomainException">Thrown when the invoice number is already set.</exception>
        public void SetInvoiceNumber(string invoiceNumber)
        {
            if (!string.IsNullOrEmpty(InvoiceNumber))
                throw new DomainException("Invoice number is already set.");
            InvoiceNumber = invoiceNumber;
        }
    }
}