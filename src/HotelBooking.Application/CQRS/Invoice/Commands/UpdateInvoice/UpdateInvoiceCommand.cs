using FluentValidation;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.Commands.CreateInvoice;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Commands.UpdateInvoice;

public record UpdateInvoiceCommand : IRequest<Result>
{
    public int Id { get; init; }
    public DateTime DueDate { get; init; }
    public string PaymentMethod { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public List<InvoiceItemDto> Items { get; init; } = new();
}

public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
{
    public UpdateInvoiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Invoice ID must be greater than 0");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required")
            .GreaterThan(DateTime.Today)
            .WithMessage("Due date must be in the future");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Payment method is required")
            .MaximumLength(50)
            .WithMessage("Payment method cannot exceed 50 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes cannot exceed 500 characters");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");

        RuleForEach(x => x.Items).SetValidator(new InvoiceItemDtoValidator());
    }
} 