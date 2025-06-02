using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Queries
{
    public class GetPendingInvoicesQueryHandler : IRequestHandler<GetPendingInvoicesQuery, Result<List<InvoiceDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public GetPendingInvoicesQueryHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<InvoiceDto>>> Handle(GetPendingInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepository.GetPendingInvoicesAsync(request.FromDate, request.ToDate);
            var invoiceDtos = _mapper.Map<List<InvoiceDto>>(invoices);
            return Result<List<InvoiceDto>>.Success(invoiceDtos);
        }
    }
} 