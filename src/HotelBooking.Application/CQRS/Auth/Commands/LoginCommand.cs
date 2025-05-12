using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Auth.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Auth.Commands
{
    public class LoginCommand : IRequest<Result<LoginResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
} 