using HotelBooking.Application.CQRS.Building.DTOs;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public record GetBuildingByIdQuery(int Id) : IQuery<Result<BuildingDto>>;
} 