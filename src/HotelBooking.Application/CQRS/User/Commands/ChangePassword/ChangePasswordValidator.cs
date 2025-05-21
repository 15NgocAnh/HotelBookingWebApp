using FluentValidation;

namespace HotelBooking.Application.CQRS.User.Commands.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Password.CurrentPassword)
                .NotEmpty().WithMessage("Mật khẩu hiện tại không được để trống.");

            RuleFor(x => x.Password.NewPassword)
                .NotEmpty().WithMessage("Mật khẩu mới không được để trống.")
                .NotEqual(x => x.Password.CurrentPassword)
                .WithMessage("Mật khẩu mới không được trùng với mật khẩu hiện tại.");

            RuleFor(x => x.Password.ConfirmPassword)
                .Equal(x => x.Password.NewPassword)
                .WithMessage("Xác nhận mật khẩu không khớp.");
        }
    }
}
