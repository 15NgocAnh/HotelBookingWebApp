using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;

namespace HotelBooking.Application.CQRS.Invoice.Queries
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, Result<InvoiceDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetInvoiceByIdQueryHandler(
            IInvoiceRepository invoiceRepository,
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<InvoiceDto>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(request.Id);
            if (invoice == null)
                return Result<InvoiceDto>.Failure("Invoice not found");

            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
            var user = await _userRepository.GetByIdAsync(int.Parse(invoiceDto.CreatedBy));
            invoiceDto.CreatedBy = $"{user?.FirstName} {user?.LastName} (NV{user?.Id:0000})";

            var booking = await _bookingRepository.GetByIdAsync(invoiceDto.BookingId);
            invoiceDto.Guests = _mapper.Map<List<GuestDto>>(booking.Guests);

            return Result<InvoiceDto>.Success(invoiceDto);
        }
    }
} 