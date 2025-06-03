using HotelBooking.Domain.AggregateModels.InvoiceAggregate;

namespace HotelBooking.Application.CQRS.Invoice.Commands.UpdateInvoiceStatus
{
    public record UpdateInvoiceStatusCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public InvoiceStatus Status { get; init; }
    }
}