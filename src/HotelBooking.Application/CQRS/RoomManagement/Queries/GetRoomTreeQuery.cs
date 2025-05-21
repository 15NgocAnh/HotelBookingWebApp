using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.RoomManagement.Queries
{
    public record GetRoomTreeQuery : IQuery<Result<List<object>>>;
} 