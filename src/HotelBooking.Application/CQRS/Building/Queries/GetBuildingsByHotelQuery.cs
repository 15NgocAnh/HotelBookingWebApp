using HotelBooking.Application.CQRS.Building.DTOs;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public record GetBuildingsByHotelQuery(int HotelId) : IQuery<Result<List<BuildingDto>>>;
} 