using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Hotel.Commands
{
    public record DeleteHotelCommand(int Id) : IRequest<Result>;
} 