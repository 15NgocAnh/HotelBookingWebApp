using HotelBooking.Domain.Common;

namespace HotelBooking.Application.CQRS.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHotelRepository _userHotelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(
            IUserRepository userRepository,
            IUserHotelRepository userHotelRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userHotelRepository = userHotelRepository;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            await _userRepository.SoftDeleteAsync(user);
            await _userHotelRepository.DeleteAllByUserIdAsync(request.Id);

            await _unitOfWork.SaveEntitiesAsync(cancellationToken);

            return Result.Success();
        }
    }
} 