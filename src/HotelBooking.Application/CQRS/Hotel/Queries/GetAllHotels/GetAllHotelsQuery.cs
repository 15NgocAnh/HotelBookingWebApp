using FluentValidation;
using HotelBooking.Application.CQRS.Hotel.DTOs;

namespace HotelBooking.Application.CQRS.Hotel.Queries.GetAllHotels
{
    public record GetAllHotelsQuery : ICommand<Result<List<HotelDto>>>
    {
        public List<int> HotelIds { get; set; } = new();
    }

    public class GetAllHotelsQueryValidator : AbstractValidator<GetAllHotelsQuery>
    {
        public GetAllHotelsQueryValidator()
        {
            RuleForEach(x => x.HotelIds)
                .GreaterThan(0).WithMessage("Hotel ID must be greater than 0");
        }
    }
}