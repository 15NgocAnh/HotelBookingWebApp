using FluentValidation;
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Invoice.Commands.CreateInvoice;

public record CreateInvoiceCommand : ICommand<Result<int>>
{
    public int BookingId { get; init; }
    public DateTime DueDate { get; init; }
    public string PaymentMethod { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public List<InvoiceItemDto> Items { get; init; } = new();
}

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0)
            .WithMessage("Booking ID must be greater than 0");

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

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");

        RuleForEach(x => x.Items).SetValidator(new InvoiceItemDtoValidator());
    }
}

public class InvoiceItemDtoValidator : AbstractValidator<InvoiceItemDto>
{
    public InvoiceItemDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(200)
            .WithMessage("Description cannot exceed 200 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than 0");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type is required")
            .MaximumLength(50)
            .WithMessage("Type cannot exceed 50 characters");
    }
} 