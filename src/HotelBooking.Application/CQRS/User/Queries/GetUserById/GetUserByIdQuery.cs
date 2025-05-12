using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.User.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.User.Queries.GetUserById
{
    public record GetUserByIdQuery : IRequest<Result<UserDto>>
    {
        public int Id { get; init; }
    }
} 