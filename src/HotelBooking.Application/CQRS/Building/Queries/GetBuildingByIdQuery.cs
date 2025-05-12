using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Building.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public record GetBuildingByIdQuery(int Id) : IRequest<Result<BuildingDto>>;
} 