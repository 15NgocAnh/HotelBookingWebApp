using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Commands
{
    public record UpdateBookingStatusCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public BookingStatus Status { get; init; }
    }
} 