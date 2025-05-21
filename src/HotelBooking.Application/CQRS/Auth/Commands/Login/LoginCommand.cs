using HotelBooking.Application.CQRS.Auth.DTOs;

namespace HotelBooking.Application.CQRS.Auth.Commands.Login
{
    public class LoginCommand : IRequest<Result<LoginResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}