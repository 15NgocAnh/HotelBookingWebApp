using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Hotel.DTOs;

namespace HotelBooking.Application.CQRS.Hotel.Queries.GetAllHotels
{
    public record GetAllHotelsQuery : ICommand<Result<List<HotelDto>>>;
}