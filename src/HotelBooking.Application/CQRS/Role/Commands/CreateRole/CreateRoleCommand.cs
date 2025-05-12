using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Role.Commands.CreateRole
{
    public record CreateRoleCommand : IRequest<Result<int>>
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
} 