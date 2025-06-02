using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.User.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.User.Queries.GetAllUsers
{
    public record GetAllUsersQuery : IRequest<Result<List<UserDto>>>
    {
        public bool IncludeInactive { get; init; }
        public string SearchTerm { get; init; }
        public string RoleName { get; init; }
        public int? PageNumber { get; init; }
        public int? PageSize { get; init; }
        public string SortBy { get; init; }
        public bool SortDescending { get; init; }
    }
} 