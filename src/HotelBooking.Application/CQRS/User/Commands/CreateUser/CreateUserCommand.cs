using FluentValidation;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.User.Commands.CreateUser
{
    public record CreateUserCommand : ICommand<Result<int>>
    {
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string? PhoneNumber { get; init; }
        public int RoleId { get; set; }
        public List<int> HotelIds { get; init; } = new List<int>();
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");
        }
    }
} 