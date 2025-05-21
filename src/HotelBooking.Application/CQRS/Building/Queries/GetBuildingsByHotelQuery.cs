using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Building.DTOs;
using System.Collections.Generic;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public record GetBuildingsByHotelQuery(int HotelId) : IQuery<Result<List<BuildingDto>>>;
} 