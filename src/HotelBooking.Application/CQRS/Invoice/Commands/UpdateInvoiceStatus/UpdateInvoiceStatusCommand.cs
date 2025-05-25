namespace HotelBooking.Application.CQRS.Invoice.Commands.UpdateInvoiceStatus
{
    public record UpdateInvoiceStatusCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public string Status { get; init; }
        public string PaymentMethod { get; init; }
        public decimal PaidAmount { get; init; }
        public string Notes { get; init; }
    }
}