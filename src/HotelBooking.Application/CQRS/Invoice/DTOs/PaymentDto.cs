using HotelBooking.Domain.Utils.Enum;

namespace HotelBooking.Application.CQRS.Invoice.DTOs
{
    public class PaymentDto
    {
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
