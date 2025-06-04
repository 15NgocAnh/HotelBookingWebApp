using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Application.Services.User;
using HotelBooking.Domain.Utils.Enum;

namespace HotelBooking.Application.CQRS.User.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetUserByIdQueryHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IHotelRepository hotelRepository,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.Id);
                if (user == null)
                {
                    return Result<UserDto>.Failure("User not found");
                }

                // Check if current user has permission to view this user
                if (_currentUserService.Role != Domain.Utils.Enum.Role.SuperAdmin.ToString() && user.CreatedBy != _currentUserService.UserId.ToString())
                {
                    return Result<UserDto>.Failure("You don't have permission to view this user");
                }

                user.Role = await _roleRepository.GetByIdAsync(user.RoleId);

                var userDto = _mapper.Map<UserDto>(user);

                var userHotels = await _hotelRepository.GetAllByUserIdAsync(userDto.Id);
                userDto.Hotels = _mapper.Map<List<HotelDto>>(userHotels);

                // Kiểm tra quyền truy cập hotel
                if (request.HotelIds != null && request.HotelIds.Any())
                {
                    // Nếu user không phải SuperAdmin, chỉ lấy các hotel mà user có quyền truy cập
                    if (user.Role?.Name != "SuperAdmin")
                    {
                        userDto.Hotels = userDto.Hotels.Where(h => request.HotelIds.Contains(h.Id)).ToList();
                    }
                }

                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure("Something went wrong");
            }
        }
    }
} 