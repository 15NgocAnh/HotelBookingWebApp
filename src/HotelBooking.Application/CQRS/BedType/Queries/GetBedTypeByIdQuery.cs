using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.BedType.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Queries
{
    public record GetBedTypeByIdQuery(int Id) : IRequest<Result<BedTypeDto>>;
} 