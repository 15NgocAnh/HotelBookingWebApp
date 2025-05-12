using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Commands
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<int>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateInvoiceCommandHandler(
            IInvoiceRepository invoiceRepository,
            IBookingRepository bookingRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate booking exists
                var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
                if (booking == null)
                    return Result<int>.Failure("Booking not found");

                // Check if booking already has a pending invoice
                if (await _invoiceRepository.HasPendingInvoiceForBookingAsync(request.BookingId))
                    return Result<int>.Failure("Booking already has a pending invoice");

                // Validate due date
                if (request.DueDate < DateTime.UtcNow)
                    return Result<int>.Failure("Due date cannot be in the past");

                // Create invoice
                var invoice = new Domain.AggregateModels.InvoiceAggregate.Invoice(
                    request.BookingId,
                    request.DueDate,
                    request.PaymentMethod,
                    request.Notes);

                // Add items
                foreach (var item in request.Items)
                {
                    invoice.AddItem(
                        item.Description,
                        item.Quantity,
                        item.UnitPrice,
                        item.Type);
                }

                await _invoiceRepository.AddAsync(invoice);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<int>.Success(invoice.Id);
            }
            catch (DomainException ex)
            {
                return Result<int>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error creating invoice: {ex.Message}");
            }
        }
    }
} 