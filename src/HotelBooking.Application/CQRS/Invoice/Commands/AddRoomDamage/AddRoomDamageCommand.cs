using FluentValidation;
using HotelBooking.Domain.Common;

namespace HotelBooking.Application.CQRS.Invoice.Commands.AddRoomDamage;

public class AddRoomDamageCommand : ICommand<Result>
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class AddRoomDamageCommandValidator : AbstractValidator<AddRoomDamageCommand>
{
    public AddRoomDamageCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .GreaterThan(0).WithMessage("Invoice ID must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
    }
} 