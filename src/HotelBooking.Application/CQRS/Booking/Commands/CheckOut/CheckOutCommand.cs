using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Commands.CheckOut
{
    public record CheckOutCommand : IRequest<Result>
    {
        public int Id { get; init; }
    }
}