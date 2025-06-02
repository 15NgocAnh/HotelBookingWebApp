using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Domain.Common;

namespace HotelBooking.Application.CQRS.User.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                    return Result.Failure("User not found.");

                if (!_passwordHasher.VerifyPassword(request.Password.CurrentPassword, user.PasswordHash))
                    return Result.Failure("Current password is incorrect.");

                user.ChangePassword(_passwordHasher.HashPassword(request.Password.NewPassword));
                await _unitOfWork.SaveEntitiesAsync();
                return Result.Success();
            }
            catch
            {
                return Result.Failure("Something went wrong");
            }
        }
    }
}