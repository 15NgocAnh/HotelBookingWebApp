using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.BedType.Commands.CreateBedType
{
    public record CreateBedTypeCommand : ICommand<Result<int>>
    {
        public string Name { get; init; }
    }
}