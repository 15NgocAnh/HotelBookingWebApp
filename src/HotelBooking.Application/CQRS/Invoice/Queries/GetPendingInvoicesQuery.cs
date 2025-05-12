using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.DTOs;

namespace HotelBooking.Application.CQRS.Invoice.Queries
{
    public record GetPendingInvoicesQuery : IQuery<Result<List<InvoiceDto>>>
    {
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate { get; init; }
    }
} 