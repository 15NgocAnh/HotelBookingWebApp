using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Commands
{
    public class UpdateInvoiceStatusCommandHandler : IRequestHandler<UpdateInvoiceStatusCommand, Result>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInvoiceStatusCommandHandler(
            IInvoiceRepository invoiceRepository,
            IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateInvoiceStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = await _invoiceRepository.GetByIdWithItemsAsync(request.Id);
                if (invoice == null)
                    return Result.Failure("Invoice not found");

                if (!Enum.TryParse<InvoiceStatus>(request.Status, out var newStatus))
                    return Result.Failure("Invalid invoice status");

                invoice.UpdateStatus(newStatus, request.PaidAmount, request.PaymentMethod, request.Notes);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating invoice status: {ex.Message}");
            }
        }
    }
} 