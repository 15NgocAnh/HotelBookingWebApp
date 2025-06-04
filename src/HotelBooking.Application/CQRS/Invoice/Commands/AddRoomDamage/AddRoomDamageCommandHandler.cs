using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;

namespace HotelBooking.Application.CQRS.Invoice.Commands.AddRoomDamage;

public class AddRoomDamageCommandHandler : IRequestHandler<AddRoomDamageCommand, Result>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddRoomDamageCommandHandler(
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddRoomDamageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            if (invoice == null)
            {
                return Result.Failure("Invoice not found.");
            }

            if (invoice.Status == InvoiceStatus.Cancelled)
            {
                return Result.Failure("Cannot add damage charges to a cancelled invoice.");
            }

            invoice.AddItem($"Room Damage: {request.Description}", 1, request.Amount, "Damage");
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Something went wrong", ex);
        }
    }
} 