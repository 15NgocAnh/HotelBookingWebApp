using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using MediatR;
using System.Collections.Generic;

namespace HotelBooking.Application.CQRS.ExtraItem.Queries
{
    public record GetExtraItemsByCategoryQuery(int CategoryId) : IRequest<Result<List<ExtraItemDto>>>;
} 