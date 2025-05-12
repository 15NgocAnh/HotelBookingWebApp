using HotelBooking.Domain.AggregateModels.InvoiceAggregate;

namespace HotelBooking.Application.CQRS.Invoice.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public string Notes { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
    }
} 