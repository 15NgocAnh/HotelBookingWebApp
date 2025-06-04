using FluentValidation;

namespace HotelBooking.Application.CQRS.Invoice.Commands.GenerateInvoicePdf;

public class GenerateInvoicePdfCommand : ICommand<Result<byte[]>>
{
    public int InvoiceId { get; set; }
}

public class GenerateInvoicePdfCommandValidator : AbstractValidator<GenerateInvoicePdfCommand>
{
    public GenerateInvoicePdfCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .GreaterThan(0).WithMessage("Invoice ID must be greater than 0");
    }
} 