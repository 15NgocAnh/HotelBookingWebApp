using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.CQRS.Invoice.Commands.UpdateInvoice;

public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Result>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateInvoiceCommandHandler> _logger;

    public UpdateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateInvoiceCommandHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
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
                throw new DomainException("Cannot update a cancelled invoice");
            }

            if (invoice.Status == InvoiceStatus.Paid)
            {
                throw new DomainException("Cannot update a paid invoice");
            }

            // Create invoice items from DTOs
            var invoiceItems = request.Items.Select(item => new InvoiceItem(
                item.Description,
                item.Quantity,
                item.UnitPrice,
                item.Type
            )).ToList();

            // Update invoice
            invoice.Update(
                request.DueDate,
                request.PaymentMethod,
                request.Notes
            );
            invoice.AddRangeItem(invoiceItems);

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated invoice {InvoiceId}", request.Id);

            return Result.Success();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error while updating invoice {InvoiceId}", request.Id);
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating invoice {InvoiceId}", request.Id);
            return Result.Failure("An error occurred while updating the invoice");
        }
    }
} 