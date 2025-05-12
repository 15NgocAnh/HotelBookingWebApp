using AutoMapper;
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Queries
{
    public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, Result<List<InvoiceDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public GetAllInvoicesQueryHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<InvoiceDto>>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            var invoiceDtos = _mapper.Map<List<InvoiceDto>>(invoices);
            return Result<List<InvoiceDto>>.Success(invoiceDtos);
        }
    }
} 