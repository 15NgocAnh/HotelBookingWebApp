using AutoMapper;
using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Auth.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJWTHelper _jWTHelper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJWTHelper jWTHelper,
            IPasswordHasher passwordHasher,
            IMapper mapper)
        {
            _userRepository = userRepository;
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

            var token = await _jWTHelper.GenerateJWTToken(user.Id, DateTime.UtcNow.AddMinutes(10), user);
            var response = _mapper.Map<LoginResponseDto>(user);
            response.Token = token;
            response.Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            return Result<LoginResponseDto>.Success(response);
        }
    }
} 