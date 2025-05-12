using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.DTOs;

namespace HotelBooking.Application.CQRS.Invoice.Queries
{
    public record GetInvoiceByIdQuery : IQuery<Result<InvoiceDto>>
    {
        public int Id { get; init; }
    }
} 