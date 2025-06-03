using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;

namespace HotelBooking.Application.CQRS.Invoice.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public string CreatedBy { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public string? Notes { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
        public List<GuestDto> Guests { get; set; } = new();
        public List<PaymentDto> Payments { get; set; } = new();
    }
} 