using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Queries.GetBookingsByCustomer
{
    public record GetBookingsByCustomerQuery : IRequest<Result<List<BookingDto>>>
    {
        public string GuestCitizenIdNumber { get; init; }
    }
}