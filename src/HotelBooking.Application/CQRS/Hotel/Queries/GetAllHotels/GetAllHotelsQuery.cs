using HotelBooking.Application.CQRS.Hotel.DTOs;

namespace HotelBooking.Application.CQRS.Hotel.Queries.GetAllHotels
{
    public record GetAllHotelsQuery : ICommand<Result<List<HotelDto>>>
    {
        public List<int> HotelIds { get; set; } = new();
    }
}