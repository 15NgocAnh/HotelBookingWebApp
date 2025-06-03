using FluentValidation;
using HotelBooking.Application.CQRS.Invoice.DTOs;

namespace HotelBooking.Application.CQRS.Invoice.Commands.CreateInvoice;

public record CreateInvoiceCommand : ICommand<Result<int>>
{
    public int BookingId { get; init; }
    public string? Notes { get; init; }
}

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0)
            .WithMessage("Booking ID must be greater than 0");
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