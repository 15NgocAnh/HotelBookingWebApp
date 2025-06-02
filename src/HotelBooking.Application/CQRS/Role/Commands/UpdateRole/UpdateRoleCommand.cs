using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Role.Commands.UpdateRole
{
    public record UpdateRoleCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
    }
} 