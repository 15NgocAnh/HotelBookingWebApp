using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.Hotel.Commands.DeleteHotel
{
    public record DeleteHotelCommand(int Id) : ICommand<Result>;
}