using HotelBooking.Application.CQRS.Auth.DTOs;

namespace HotelBooking.Application.CQRS.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<Result<LoginResponseDto>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
} 