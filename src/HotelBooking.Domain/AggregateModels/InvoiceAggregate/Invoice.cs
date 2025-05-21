using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using System.Xml.Linq;

namespace HotelBooking.Domain.AggregateModels.InvoiceAggregate
{
    public class Invoice : BaseEntity, IAggregateRoot
    {
        public int BookingId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public decimal PaidAmount { get; private set; }
        public decimal RemainingAmount { get; private set; }
        public InvoiceStatus Status { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? PaidDate { get; private set; }
        public string PaymentMethod { get; private set; }
        public string Notes { get; private set; }
        public bool IsLatePayment { get; private set; }
        public decimal LatePaymentFee { get; private set; }
        public string? CancellationReason { get; private set; }
        public DateTime? CancellationDate { get; private set; }

        private readonly List<InvoiceItem> _items = [];
        public IReadOnlyCollection<InvoiceItem> Items => _items.AsReadOnly();

        private readonly List<PaymentRecord> _payments = [];
        public IReadOnlyCollection<PaymentRecord> Payments => _payments.AsReadOnly();

        private Invoice() { } // For EF Core

        public Invoice(int bookingId, DateTime dueDate, string paymentMethod, string notes)
        {
            BookingId = bookingId;
            DueDate = dueDate;
            PaymentMethod = paymentMethod;
            Notes = notes;
            Status = InvoiceStatus.Pending;
            PaidAmount = 0;
            RemainingAmount = 0;
            LatePaymentFee = 0;
        }

        public void Update(DateTime dueDate, string paymentMethod, string notes)
        {
            DueDate = dueDate;
            PaymentMethod = paymentMethod;
            Notes = notes;
        }

        public void AddItem(string description, int quantity, decimal unitPrice, string type)
        {
            var item = new InvoiceItem(description, quantity, unitPrice, type);
            _items.Add(item);
            UpdateTotalAmount();
        }

        public void AddRangeItem(List<InvoiceItem> invoiceItems)
        {
            _items.Clear();
            _items.AddRange(invoiceItems);
            UpdateTotalAmount();
        }

        public void AddPayment(decimal amount, string paymentMethod, string? notes = null)
        {
            if (amount <= 0)
                throw new DomainException("Payment amount must be greater than zero");

            if (Status == InvoiceStatus.Cancelled)
                throw new DomainException("Cannot add payment to a cancelled invoice");

            var payment = new PaymentRecord(amount, paymentMethod, notes);
            _payments.Add(payment);

            PaidAmount += amount;
            UpdateStatus();

            if (PaidAmount >= TotalAmount)
            {
                PaidDate = DateTime.UtcNow;
            }

            SetUpdatedAt();
        }

        public void UpdateStatus(InvoiceStatus newStatus, decimal paidAmount, string paymentMethod, string notes)
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
                    if (paidAmount < TotalAmount)
                        throw new DomainException("Cannot mark as Paid with partial payment");
                    PaidDate = DateTime.UtcNow;
                    break;

                case InvoiceStatus.PartiallyPaid:
                    if (Status == InvoiceStatus.Cancelled)
                        throw new DomainException("Cannot mark a cancelled invoice as PartiallyPaid");
                    if (paidAmount >= TotalAmount)
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
            PaymentMethod = paymentMethod;
            Notes = notes;
            SetUpdatedAt();
        }

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

        private void UpdateTotalAmount()
        {
            TotalAmount = _items.Sum(item => item.TotalPrice);
            RemainingAmount = TotalAmount - PaidAmount;
            UpdateStatus();
        }

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

        private void CalculateLatePaymentFee()
        {
            if (!IsLatePayment)
                return;

            var daysLate = (DateTime.UtcNow - DueDate).Days;
            LatePaymentFee = Math.Round(RemainingAmount * 0.01m * daysLate, 2); // 1% per day late
        }
    }
}