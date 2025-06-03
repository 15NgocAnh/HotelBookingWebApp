using HotelBooking.Domain.Utils.Enum;

namespace HotelBooking.Application.CQRS.Invoice.Commands.AddPayment;

public class AddPaymentCommand : ICommand<Result>
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
} 