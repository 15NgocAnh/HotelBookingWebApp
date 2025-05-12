using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.User.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Phone { get; init; }
        public List<int> RoleIds { get; init; }
    }
} 