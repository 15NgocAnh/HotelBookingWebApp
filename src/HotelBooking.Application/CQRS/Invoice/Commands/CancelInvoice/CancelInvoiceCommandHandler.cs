using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.CQRS.Invoice.Commands.CancelInvoice;

public class CancelInvoiceCommandHandler : IRequestHandler<CancelInvoiceCommand, Result>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelInvoiceCommandHandler> _logger;

    public CancelInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork,
        ILogger<CancelInvoiceCommandHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(CancelInvoiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var invoice = await _invoiceRepository.GetByIdAsync(request.Id);
            if (invoice == null)
            {
                throw new NotFoundException($"Invoice with ID {request.Id} not found");
            }

            if (invoice.Status == InvoiceStatus.Cancelled)
            {
                throw new DomainException("Invoice is already cancelled");
            }

            if (invoice.Status == InvoiceStatus.Paid)
            {
                throw new DomainException("Cannot cancel a paid invoice");
            }

            // Cancel the invoice
            invoice.Cancel(request.CancellationReason);

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Cancelled invoice {InvoiceId}", request.Id);

            return Result.Success();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error while cancelling invoice {InvoiceId}", request.Id);
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling invoice {InvoiceId}", request.Id);
            return Result.Failure("An error occurred while cancelling the invoice");
        }
    }
} 