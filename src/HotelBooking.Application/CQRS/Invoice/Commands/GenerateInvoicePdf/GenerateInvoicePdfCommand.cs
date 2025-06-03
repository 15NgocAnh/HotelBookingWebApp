namespace HotelBooking.Application.CQRS.Invoice.Commands.GenerateInvoicePdf;

public class GenerateInvoicePdfCommand : ICommand<Result<byte[]>>
{
    public int InvoiceId { get; set; }
} 