using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraCategory.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraCategory.Queries
{
    public record GetExtraCategoryByIdQuery(int Id) : IRequest<Result<ExtraCategoryDto>>;
} 