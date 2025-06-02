using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Role.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Role.Queries.GetRoleById
{
    public record GetRoleByIdQuery : IRequest<Result<RoleDto>>
    {
        public int Id { get; init; }
    }
} 