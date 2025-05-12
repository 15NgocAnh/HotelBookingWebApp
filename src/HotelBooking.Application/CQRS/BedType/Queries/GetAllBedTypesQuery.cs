using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.BedType.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Queries
{
    public record GetAllBedTypesQuery : IRequest<Result<List<BedTypeDto>>>;
} 