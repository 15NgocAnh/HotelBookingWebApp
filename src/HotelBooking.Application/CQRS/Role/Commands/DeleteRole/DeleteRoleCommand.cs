using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Role.Commands.DeleteRole
{
    public record DeleteRoleCommand : IRequest<Result>
    {
        public int Id { get; init; }
    }
} 