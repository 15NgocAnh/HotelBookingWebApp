using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Role.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Role.Queries.GetAllRoles
{
    public record GetAllRolesQuery : IRequest<Result<List<RoleDto>>>
    {
        public bool IncludeInactive { get; init; }
    }
} 