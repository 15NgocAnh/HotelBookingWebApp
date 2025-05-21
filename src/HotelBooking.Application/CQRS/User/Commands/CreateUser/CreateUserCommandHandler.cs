using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Application.CQRS.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<int>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserHotelRepository _userHotelRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IHotelRepository hotelRepository,
            IUserHotelRepository userHotelRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _hotelRepository = hotelRepository;
            _userHotelRepository = userHotelRepository;
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.IsEmailUniqueAsync(request.Email))
            {
                return Result<int>.Failure("Email already exists");
            }

            var hashedPassword = _passwordHasher.HashPassword(request.Password);
            var user = new Domain.AggregateModels.UserAggregate.User(
                request.Email,
                hashedPassword,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.RoleId
            );

            await _userRepository.AddAsync(user);

            foreach (var hotelId in request.HotelIds)
            {
                var hotel = await _hotelRepository.GetByIdAsync(hotelId);
                if (hotel != null)
                {
                    var userHotel = new UserHotel(user, hotel);
                    await _userHotelRepository.AddAsync(userHotel);
                }
            }

            return Result<int>.Success(user.Id);
        }
    }
} 