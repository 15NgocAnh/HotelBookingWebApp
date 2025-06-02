using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.BedType.Commands.DeleteBedType
{
    public record DeleteBedTypeCommand(int Id) : ICommand<Result>;
}