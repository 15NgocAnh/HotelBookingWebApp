using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.User.DTOs;

namespace HotelBooking.Application.CQRS.User.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                return Result<UserDto>.Failure("User not found");
            }

            user.Role = await _roleRepository.GetByIdAsync(user.RoleId);

            var userDto = _mapper.Map<UserDto>(user);

            userDto.Hotels = _mapper.Map<List<HotelDto>>(await _hotelRepository.GetAllByUserIdAsync(userDto.Id));

            return Result<UserDto>.Success(userDto);
        }
    }
} 