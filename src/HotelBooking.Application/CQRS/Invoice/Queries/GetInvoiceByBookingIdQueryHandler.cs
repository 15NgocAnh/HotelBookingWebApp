using AutoMapper;
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Queries
{
    public class GetInvoiceByBookingIdQueryHandler : IRequestHandler<GetInvoiceByBookingIdQuery, Result<InvoiceDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public GetInvoiceByBookingIdQueryHandler(
            IInvoiceRepository invoiceRepository,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public async Task<Result<InvoiceDto>> Handle(GetInvoiceByBookingIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetByBookingIdAsync(request.BookingId);
            if (invoice == null)
                return Result<InvoiceDto>.Failure("Invoice not found for the specified booking");

            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
            return Result<InvoiceDto>.Success(invoiceDto);
        }
    }
} 