using HotelBooking.Application.CQRS.Building.DTOs;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public record GetAllFloorsByBuildingIdQuery(int Id) : IQuery<Result<List<FloorDto>>>
    {
        public List<int> HotelIds { get; set; } = new();
    }
} 