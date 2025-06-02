using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;

namespace HotelBooking.Application.CQRS.ExtraItem.Queries
{
    public record GetExtraItemByIdQuery(int Id) : IQuery<Result<ExtraItemDto>>;
} 