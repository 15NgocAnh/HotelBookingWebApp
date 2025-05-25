using FluentValidation;
using HotelBooking.Application.CQRS.Booking.DTOs;

namespace HotelBooking.Application.CQRS.Booking.Commands.CreateBooking
{
    public record CreateBookingCommand : IRequest<Result<int>>
    {
        public int RoomId { get; init; }
        public DateTime CheckInDate { get; init; } = DateTime.UtcNow;
        public DateTime CheckOutDate { get; init; } = DateTime.UtcNow.AddDays(1);
        public string? Notes { get; init; }
        public List<GuestDto> Guests { get; init; } = new();
        public List<ExtraUsageDto> ExtraUsages { get; init; } = new();
    }

    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.RoomId)
                .GreaterThan(0)
                .WithMessage("Room ID must be greater than 0");

            RuleFor(x => x.CheckInDate)
                .NotEmpty()
                .WithMessage("Check-in date is required")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Check-in date cannot be in the past");

            RuleFor(x => x.CheckOutDate)
                .NotEmpty();
        }
    }
}