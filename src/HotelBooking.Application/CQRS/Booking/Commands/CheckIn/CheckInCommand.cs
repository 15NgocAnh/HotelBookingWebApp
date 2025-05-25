using FluentValidation;

namespace HotelBooking.Application.CQRS.Booking.Commands.CheckIn;

public record CheckInCommand : IRequest<Result>
{
    public int Id { get; init; }
    public string? Notes { get; init; }
}

public class CheckInCommandValidator : AbstractValidator<CheckInCommand>
{
    public CheckInCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Booking ID must be greater than 0");
    }
}