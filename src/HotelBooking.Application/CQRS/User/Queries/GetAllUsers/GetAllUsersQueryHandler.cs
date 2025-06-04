using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Application.Services.User;
using HotelBooking.Domain.Utils.Enum;

namespace HotelBooking.Application.CQRS.User.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAllUsersQueryHandler(
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

        public async Task<Result<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAllAsync();

                // Apply filters
                if (!request.IncludeInactive)
                {
                    users = users.Where(u => !u.IsDeleted).ToList();
                }

                // Filter users based on role
                if (_currentUserService.Role != Domain.Utils.Enum.Role.SuperAdmin.ToString())
                {
                    // If not SuperAdmin, only show users created by the current user
                    users = users.Where(u => u.CreatedBy == _currentUserService.UserId.ToString()).ToList();
                }

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    users = users.Where(u =>
                        u.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        u.FirstName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        u.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        u.PhoneNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                // Apply sorting
                if (!string.IsNullOrWhiteSpace(request.SortBy))
                {
                    users = request.SortBy.ToLower() switch
                    {
                        "email" => request.SortDescending
                            ? users.OrderByDescending(u => u.Email).ToList()
                            : users.OrderBy(u => u.Email).ToList(),
                        "firstname" => request.SortDescending
                            ? users.OrderByDescending(u => u.FirstName).ToList()
                            : users.OrderBy(u => u.FirstName).ToList(),
                        "lastname" => request.SortDescending
                            ? users.OrderByDescending(u => u.LastName).ToList()
                            : users.OrderBy(u => u.LastName).ToList(),
                        "phone" => request.SortDescending
                            ? users.OrderByDescending(u => u.PhoneNumber).ToList()
                            : users.OrderBy(u => u.PhoneNumber).ToList(),
                        _ => users
                    };
                }

                // Apply pagination
                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    users = users
                        .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                        .Take(request.PageSize.Value)
                        .ToList();
                }

                foreach (var user in users)
                {
                    user.Role = await _roleRepository.GetByIdAsync(user.RoleId);
                }

                var userDtos = _mapper.Map<List<UserDto>>(users);
                foreach (var userDto in userDtos)
                {
                    userDto.Hotels = _mapper.Map<List<HotelDto>>(await _hotelRepository.GetAllByUserIdAsync(userDto.Id));
                }
                return Result<List<UserDto>>.Success(userDtos);
            }
            catch (Exception ex) 
            {
                return Result<List<UserDto>>.Failure("Something went wrong");
            }
        }
    }
} 