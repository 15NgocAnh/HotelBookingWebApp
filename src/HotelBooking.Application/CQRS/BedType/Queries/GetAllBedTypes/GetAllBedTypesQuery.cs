using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.BedType.DTOs;

namespace HotelBooking.Application.CQRS.BedType.Queries.GetAllBedTypes
{
    public record GetAllBedTypesQuery : IQuery<Result<List<BedTypeDto>>>;
}