using FluentValidation;
using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Commands.CancelInvoice;

public record CancelInvoiceCommand : IRequest<Result>
{
    public int Id { get; init; }
    public string? CancellationReason { get; init; }
}

public class CancelInvoiceCommandValidator : AbstractValidator<CancelInvoiceCommand>
{
    public CancelInvoiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Invoice ID must be greater than 0");

        RuleFor(x => x.CancellationReason)
            .MaximumLength(500)
            .WithMessage("Cancellation reason cannot exceed 500 characters");
    }
} 