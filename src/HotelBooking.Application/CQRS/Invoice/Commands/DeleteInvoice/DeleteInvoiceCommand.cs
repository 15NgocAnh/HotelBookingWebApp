namespace HotelBooking.Application.CQRS.Invoice.Commands.DeleteInvoice;

public class DeleteInvoiceCommand(int Id) : ICommand<Result>
{
    public int Id { get; private set; } = Id;
} 