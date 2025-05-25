using AutoMapper;
using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Auth.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserHotelRepository _userHotelRepository;
        private readonly IJWTHelper _jWTHelper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserHotelRepository userHotelRepository,
            IJWTHelper jWTHelper,
            IPasswordHasher passwordHasher,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userHotelRepository = userHotelRepository;
            _jWTHelper = jWTHelper;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return Result<LoginResponseDto>.Failure("Invalid email or password");

            if (!user.IsActive)
                return Result<LoginResponseDto>.Failure("User account is inactive");

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                return Result<LoginResponseDto>.Failure("Invalid email or password");

            user.UpdateLastLogin();
            await _userRepository.UpdateAsync(user);

            var role = await _roleRepository.GetRolesByUserIdAsync(user.Id);
            var hotels = await _userHotelRepository.GetAllByUserAsync(user.Id);

            var token = await _jWTHelper.GenerateJWTToken(user.Id, user, role.Name, hotels);
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var refreshToken = await _jWTHelper.GenerateJWTRefreshToken(user.Id, refreshTokenExpiryTime);

            var response = _mapper.Map<LoginResponseDto>(user);
            response.Token = token;
            response.RefreshToken = refreshToken;
            response.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            response.Role = role.Name;
            response.HotelIds = hotels.Select(h => h.Id).ToList();

            return Result<LoginResponseDto>.Success(response);
        }
    }
}