using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.User.DTOs;

namespace HotelBooking.Application.CQRS.User.Queries.GetUserById
{
    public record GetUserByIdQuery(int Id) : IQuery<Result<UserDto>>
    {
        public List<int> HotelIds { get; set; } = new();
    }
} 