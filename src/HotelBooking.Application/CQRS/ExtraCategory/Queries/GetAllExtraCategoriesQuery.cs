using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraCategory.DTOs;

namespace HotelBooking.Application.CQRS.ExtraCategory.Queries
{
    public record GetAllExtraCategoriesQuery : IQuery<Result<List<ExtraCategoryDto>>>;
} 