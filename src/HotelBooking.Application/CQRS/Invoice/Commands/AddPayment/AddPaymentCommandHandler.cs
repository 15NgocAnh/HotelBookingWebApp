using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.CQRS.Invoice.Commands.AddPayment;

public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, Result>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddPaymentCommandHandler> _logger;

    public AddPaymentCommandHandler(
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddPaymentCommandHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get invoice
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            if (invoice == null)
            {
                throw new NotFoundException($"Invoice with ID {request.InvoiceId} not found");
            }

            // Validate invoice status
            if (invoice.Status == InvoiceStatus.Cancelled)
            {
                throw new DomainException("Cannot add payment to a cancelled invoice");
            }

            if (invoice.Status == InvoiceStatus.Paid)
            {
                throw new DomainException("Cannot add payment to a fully paid invoice");
            }

            // Add payment to invoice
            invoice.AddPayment(request.Amount, request.PaymentMethod, request.Notes);

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added payment of {Amount} to invoice {InvoiceId}", 
                request.Amount, request.InvoiceId);

            return Result.Success();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error while adding payment to invoice {InvoiceId}", 
                request.InvoiceId);
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding payment to invoice {InvoiceId}", 
                request.InvoiceId);
            return Result.Failure("An error occurred while adding the payment");
        }
    }
} 