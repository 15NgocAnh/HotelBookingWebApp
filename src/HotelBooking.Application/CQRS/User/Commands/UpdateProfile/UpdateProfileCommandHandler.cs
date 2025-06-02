using HotelBooking.Domain.Common;

namespace HotelBooking.Application.CQRS.User.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProfileCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                    return Result.Failure("User not found.");

                user.UpdateProfile(request.Profile.FirstName, request.Profile.LastName, request.Profile.PhoneNumber);
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