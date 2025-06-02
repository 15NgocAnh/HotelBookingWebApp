using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Queries
{
    public class GetOverdueInvoicesQueryHandler : IRequestHandler<GetOverdueInvoicesQuery, Result<List<InvoiceDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public GetOverdueInvoicesQueryHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<InvoiceDto>>> Handle(GetOverdueInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepository.GetOverdueInvoicesAsync(request.FromDate, request.ToDate);
            var invoiceDtos = _mapper.Map<List<InvoiceDto>>(invoices);
            return Result<List<InvoiceDto>>.Success(invoiceDtos);
        }
    }
} 