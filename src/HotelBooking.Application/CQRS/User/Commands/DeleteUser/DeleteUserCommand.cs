using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.User.Commands.DeleteUser
{
    public record DeleteUserCommand : IRequest<Result>
    {
        public int Id { get; init; }
    }
} 