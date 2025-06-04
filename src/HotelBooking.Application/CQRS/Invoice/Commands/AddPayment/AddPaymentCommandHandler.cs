using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.Interfaces.Repositories;

namespace HotelBooking.Application.CQRS.Invoice.Commands.AddPayment;

public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, Result>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPaymentCommandHandler(
        IInvoiceRepository invoiceRepository, 
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            if (invoice == null)
            {
                return Result.Failure("Invoice not found.");
            }

            if (invoice.Status == InvoiceStatus.Paid)
            {
                return Result.Failure("Invoice is already fully paid.");
            }

            if (invoice.Status == InvoiceStatus.Cancelled)
            {
                return Result.Failure("Cannot add payment to a cancelled invoice.");
            }

            if (request.Amount <= 0)
            {
                return Result.Failure("Payment amount must be greater than 0.");
            }

            if (request.Amount > invoice.RemainingAmount)
            {
                return Result.Failure("Payment amount cannot exceed the remaining amount.");
            }

            invoice.AddPayment(request.Amount, request.PaymentMethod);
            
            // If the invoice is now fully paid, update the booking status to completed
            if (invoice.Status == InvoiceStatus.Paid)
            {
                var booking = await _bookingRepository.GetByIdAsync(invoice.BookingId);
                if (booking != null)
                {
                    booking.UpdateStatus(BookingStatus.Completed);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Something went wrong", ex);
        }
    }
} 