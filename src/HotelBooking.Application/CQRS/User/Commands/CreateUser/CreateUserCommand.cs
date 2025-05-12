using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.User.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<Result<int>>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Phone { get; init; }
        public List<int> RoleIds { get; init; }
    }
} 