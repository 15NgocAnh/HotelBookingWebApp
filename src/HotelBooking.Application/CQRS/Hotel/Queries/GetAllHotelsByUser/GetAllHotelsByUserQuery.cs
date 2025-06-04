using HotelBooking.Application.CQRS.Hotel.DTOs;

namespace HotelBooking.Application.CQRS.Hotel.Queries.GetAllHotels
{
    public record GetAllHotelsByUserQuery(int id) : ICommand<Result<List<HotelDto>>>;
}