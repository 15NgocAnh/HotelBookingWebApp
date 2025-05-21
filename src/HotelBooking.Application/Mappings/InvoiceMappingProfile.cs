using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;

namespace HotelBooking.Application.Mappings
{
    public class InvoiceMappingProfile : Profile
    {
        public InvoiceMappingProfile()
        {
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<Payment, PaymentDto>();
        }
    }
}