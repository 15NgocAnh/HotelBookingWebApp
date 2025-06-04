using FluentValidation;

namespace HotelBooking.Application.CQRS.Amenity.Commands.CreateAmenity
{
    public record CreateAmenityCommand : ICommand<Result<int>>
    {
        public string Name { get; init; }
    }

    public class CreateAmenityCommandValidator : AbstractValidator<CreateAmenityCommand>
    {
        public CreateAmenityCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
        }
    }
}