using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.DTOs;

namespace HotelBooking.Application.CQRS.Invoice.Queries
{
    public record GetInvoiceByBookingIdQuery : IQuery<Result<InvoiceDto>>
    {
        public int BookingId { get; init; }
    }
} 