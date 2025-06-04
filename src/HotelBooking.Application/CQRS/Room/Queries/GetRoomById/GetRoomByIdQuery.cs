using FluentValidation;
using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetRoomById
{
    public record GetRoomByIdQuery(int Id) : IQuery<Result<RoomDto>>
    {
        public List<int> HotelIds { get; set; } = new();
    }

    public class GetRoomByIdQueryValidator : AbstractValidator<GetRoomByIdQuery>
    {
        public GetRoomByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Room ID must be greater than 0");

            RuleForEach(x => x.HotelIds)
                .GreaterThan(0).WithMessage("Hotel ID must be greater than 0");
        }
    }
}