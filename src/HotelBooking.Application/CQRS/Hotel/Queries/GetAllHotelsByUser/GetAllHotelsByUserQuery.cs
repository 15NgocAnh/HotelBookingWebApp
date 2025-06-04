using FluentValidation;
using HotelBooking.Application.CQRS.Hotel.DTOs;

namespace HotelBooking.Application.CQRS.Hotel.Queries.GetAllHotels
{
    public record GetAllHotelsByUserQuery(int id) : ICommand<Result<List<HotelDto>>>;

    public class GetAllHotelsByUserQueryValidator : AbstractValidator<GetAllHotelsByUserQuery>
    {
        public GetAllHotelsByUserQueryValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("User ID must be greater than 0");
        }
    }
}