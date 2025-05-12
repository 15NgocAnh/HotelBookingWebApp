using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.Invoice.Commands
{
    public record UpdateInvoiceStatusCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public string Status { get; init; }
        public string PaymentMethod { get; init; }
        public decimal PaidAmount { get; init; }
        public string Notes { get; init; }
    }
} 