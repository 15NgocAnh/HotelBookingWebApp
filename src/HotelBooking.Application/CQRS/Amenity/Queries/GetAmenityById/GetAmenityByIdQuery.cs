using FluentValidation;
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Amenity.DTOs;

namespace HotelBooking.Application.CQRS.Amenity.Queries.GetAmenityById
{
    public record GetAmenityByIdQuery(int Id) : IQuery<Result<AmenityDto>>
    {
        public List<int> HotelIds { get; set; } = new();
    }

    public class GetAmenityByIdQueryValidator : AbstractValidator<GetAmenityByIdQuery>
    {
        public GetAmenityByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Amenity ID must be greater than 0");

            RuleForEach(x => x.HotelIds)
                .GreaterThan(0).WithMessage("Hotel ID must be greater than 0");
        }
    }
}