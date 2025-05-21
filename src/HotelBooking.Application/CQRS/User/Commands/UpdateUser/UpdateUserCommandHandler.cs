using HotelBooking.Domain.AggregateModels.UserAggregate;
using HotelBooking.Domain.Common;

namespace HotelBooking.Application.CQRS.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserHotelRepository _userHotelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            IHotelRepository hotelRepository,
            IUserHotelRepository userHotelRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _hotelRepository = hotelRepository;
            _userHotelRepository = userHotelRepository;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            if (user.Email != request.Email && !await _userRepository.IsEmailUniqueAsync(request.Email))
            {
                return Result.Failure("Email already exists");
            }

            user.Update(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Phone,
                request.RoleId
            );

            await _userHotelRepository.DeleteAllByUserIdAsync(request.Id);

            foreach (var hotelId in request.HotelIds)
            {
                var hotel = await _hotelRepository.GetByIdAsync(hotelId);
                if (hotel != null)
                {
                    var userHotel = new UserHotel(user, hotel);
                    await _userHotelRepository.AddAsync(userHotel);
                }
            }

            await _unitOfWork.SaveEntitiesAsync(cancellationToken);

            return Result.Success();
        }
    }
} 