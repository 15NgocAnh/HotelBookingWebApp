using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using System.Collections.Generic;

namespace HotelBooking.Application.CQRS.ExtraItem.Queries
{
    public record GetExtraItemsByCategoryQuery(int CategoryId) : IQuery<Result<List<ExtraItemDto>>>;
} 