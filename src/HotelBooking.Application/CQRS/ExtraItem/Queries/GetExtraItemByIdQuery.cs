using FluentValidation;
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;

namespace HotelBooking.Application.CQRS.ExtraItem.Queries
{
    public record GetExtraItemByIdQuery(int Id) : IQuery<Result<ExtraItemDto>>;

    public class GetExtraItemByIdQueryValidator : AbstractValidator<GetExtraItemByIdQuery>
    {
        public GetExtraItemByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Extra Item ID must be greater than 0");
        }
    }
} 