using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Building.DTOs;
using MediatR;
using System.Collections.Generic;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public record GetBuildingsByHotelQuery(int HotelId) : IRequest<Result<List<BuildingDto>>>;
} 