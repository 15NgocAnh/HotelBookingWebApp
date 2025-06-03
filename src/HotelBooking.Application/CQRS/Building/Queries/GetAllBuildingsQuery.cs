using HotelBooking.Application.CQRS.Building.DTOs;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public record GetAllBuildingsQuery : IQuery<Result<List<BuildingDto>>>
    {
        public List<int> HotelIds { get; set; } = new();
    }
} 