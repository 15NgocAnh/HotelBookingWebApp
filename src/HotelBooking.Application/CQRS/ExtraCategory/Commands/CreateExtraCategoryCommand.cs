using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.ExtraCategory.Commands
{
    public record CreateExtraCategoryCommand : ICommand<Result<int>>
    {
        public string Name { get; init; }
    }
} 