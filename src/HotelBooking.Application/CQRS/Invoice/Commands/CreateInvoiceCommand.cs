using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.DTOs;

namespace HotelBooking.Application.CQRS.Invoice.Commands
{
    public record CreateInvoiceCommand : ICommand<Result<int>>
    {
        public int BookingId { get; init; }
        public DateTime DueDate { get; init; }
        public string PaymentMethod { get; init; }
        public string Notes { get; init; }
        public List<InvoiceItemDto> Items { get; init; }
    }
} 