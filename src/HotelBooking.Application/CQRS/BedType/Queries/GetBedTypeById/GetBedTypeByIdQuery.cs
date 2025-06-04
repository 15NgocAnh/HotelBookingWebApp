using FluentValidation;
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.BedType.DTOs;

namespace HotelBooking.Application.CQRS.BedType.Queries.GetBedTypeById
{
    public record GetBedTypeByIdQuery(int Id) : IQuery<Result<BedTypeDto>>;

    public class GetBedTypeByIdQueryValidator : AbstractValidator<GetBedTypeByIdQuery>
    {
        public GetBedTypeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Bed Type ID must be greater than 0");
        }
    }
}