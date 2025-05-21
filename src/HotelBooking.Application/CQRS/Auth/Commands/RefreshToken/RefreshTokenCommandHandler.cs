using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Application.CQRS.Auth.DTOs;
using System.Security.Claims;

namespace HotelBooking.Application.CQRS.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IJWTHelper _jWTHelper;
        private readonly IMapper _mapper;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IJWTHelper jWTHelper,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jWTHelper = jWTHelper;
            _mapper = mapper;
        }

        public async Task<Result<LoginResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _jWTHelper.ValidateToken(request.AccessToken, validateLifetime: false);
            if (principal == null || !principal.Claims.Any())
                return Result<LoginResponseDto>.Failure("Invalid access token");

            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Result<LoginResponseDto>.Failure("User not found");

            if (!user.IsActive)
                return Result<LoginResponseDto>.Failure("User account is inactive");

            var role = await _roleRepository.GetRolesByUserIdAsync(user.Id);

            var newAccessToken = await _jWTHelper.GenerateJWTToken(user.Id, user, role.Name);
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var newRefreshToken = await _jWTHelper.GenerateJWTRefreshToken(user.Id, refreshTokenExpiryTime);

            var response = _mapper.Map<LoginResponseDto>(user);
            response.Token = newAccessToken;
            response.RefreshToken = newRefreshToken;
            response.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            response.Role = role.Name;

            return Result<LoginResponseDto>.Success(response);
        }
    }
} 