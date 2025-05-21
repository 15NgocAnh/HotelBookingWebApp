using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.BedType.Commands.UpdateBedType
{
    public record UpdateBedTypeCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}