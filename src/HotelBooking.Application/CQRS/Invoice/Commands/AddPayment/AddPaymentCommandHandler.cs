using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Common;

namespace HotelBooking.Application.CQRS.Invoice.Commands.AddPayment;

public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, Result>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPaymentCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
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
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Something went wrong", ex);
        }
    }
} 