using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private readonly List<InvoiceItem> _items = [];
        public IReadOnlyCollection<InvoiceItem> Items => _items.AsReadOnly();

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
        }

        public void AddItem(string description, int quantity, decimal unitPrice, string type)
        {
            var item = new InvoiceItem(description, quantity, unitPrice, type);
            _items.Add(item);
            UpdateTotalAmount();
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
                    if (Status != InvoiceStatus.Pending && Status != InvoiceStatus.PartiallyPaid)
                        throw new DomainException("Can only mark as Paid from Pending or PartiallyPaid status");
                    if (paidAmount < TotalAmount)
                        throw new DomainException("Paid amount must be equal to total amount for Paid status");
                    PaidDate = DateTime.UtcNow;
                    break;

                case InvoiceStatus.PartiallyPaid:
                    if (Status != InvoiceStatus.Pending)
                        throw new DomainException("Can only mark as PartiallyPaid from Pending status");
                    if (paidAmount >= TotalAmount)
                        throw new DomainException("Paid amount must be less than total amount for PartiallyPaid status");
                    break;

                case InvoiceStatus.Overdue:
                    if (Status != InvoiceStatus.Pending)
                        throw new DomainException("Can only mark as Overdue from Pending status");
                    if (DateTime.UtcNow <= DueDate)
                        throw new DomainException("Cannot mark as Overdue before due date");
                    break;

                case InvoiceStatus.Cancelled:
                    if (Status == InvoiceStatus.Paid)
                        throw new DomainException("Cannot cancel a paid invoice");
                    break;

                default:
                    throw new DomainException("Invalid invoice status");
            }

            Status = newStatus;
            PaidAmount = paidAmount;
            RemainingAmount = TotalAmount - paidAmount;
            PaymentMethod = paymentMethod;
            Notes = notes;
        }

        private void UpdateTotalAmount()
        {
            TotalAmount = _items.Sum(item => item.TotalPrice);
            RemainingAmount = TotalAmount - PaidAmount;
        }
    }
}