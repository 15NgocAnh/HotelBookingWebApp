using FluentValidation;
using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Commands.AddPayment;

public record AddPaymentCommand : IRequest<Result>
{
    public int InvoiceId { get; init; }
    public decimal Amount { get; init; }
    public string PaymentMethod { get; init; } = string.Empty;
    public string? TransactionId { get; init; }
    public string? Notes { get; init; }
}

public class AddPaymentCommandValidator : AbstractValidator<AddPaymentCommand>
{
    public AddPaymentCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .GreaterThan(0)
            .WithMessage("Invoice ID must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Payment method is required")
            .MaximumLength(50)
            .WithMessage("Payment method cannot exceed 50 characters");

        RuleFor(x => x.TransactionId)
            .MaximumLength(100)
            .WithMessage("Transaction ID cannot exceed 100 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters");
    }
} 