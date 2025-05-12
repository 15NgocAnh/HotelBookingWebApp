using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Queries
{
    public record GetExtraItemByIdQuery(int Id) : IRequest<Result<ExtraItemDto>>;
} 