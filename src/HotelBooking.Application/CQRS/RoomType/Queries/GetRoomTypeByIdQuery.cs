using FluentValidation;
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.RoomType.DTOs;

namespace HotelBooking.Application.CQRS.RoomType.Queries
{
    public record GetRoomTypeByIdQuery(int Id) : IQuery<Result<RoomTypeDto>>;

    public class GetRoomTypeByIdQueryValidator : AbstractValidator<GetRoomTypeByIdQuery>
    {
        public GetRoomTypeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Room Type ID must be greater than 0");
        }
    }
} 