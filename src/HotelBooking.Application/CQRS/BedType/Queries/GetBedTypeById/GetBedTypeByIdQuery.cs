using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.BedType.DTOs;

namespace HotelBooking.Application.CQRS.BedType.Queries.GetBedTypeById
{
    public record GetBedTypeByIdQuery(int Id) : IQuery<Result<BedTypeDto>>;
}