using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.Authentication;
using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Domain.Encryption;
using HotelBooking.Domain.Repository.Interfaces;
using HotelBooking.Domain.Response;
using HotelBooking.Data;
using System.Security.Claims;
using static HotelBooking.Domain.Response.EServiceResponseTypes;
using HotelBooking.Domain.Services.Interfaces;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _pwHasher;
        private readonly IJWTHelper _jWTHelper;
        private readonly IMapper _mapper;
        private readonly IEmailServices _emailServies;
        private readonly IJwtServices _jwtServices;
        private readonly AppDbContext _context;
        public AuthenticationServices(IUserRepository userRepository, IPasswordHasher pwhasher, IJWTHelper jWTHelper, IMapper mapper, IJwtServices jwtServices, IEmailServices emailServies, AppDbContext context)
        {
            _userRepository = userRepository;
            _pwHasher = pwhasher;
            _jWTHelper = jWTHelper;
            _mapper = mapper;
            _jwtServices = jwtServices;
            _emailServies = emailServies;
            _context = context;
        }

        public async Task<ServiceResponse<CredentialDTO>> LoginAsync(UserLoginDTO userdata)
        {
            var serviceResponse = new ServiceResponse<CredentialDTO>();
            try
            {
                var user = await _userRepository.getUserByEmail(userdata.email);
                if (user != null && _pwHasher.verify(userdata.password, user.PasswordHash))
                {
                    var userDTO = _mapper.Map<UserModel, UserDTO>(user);
                    string? token = await _jWTHelper.GenerateJWTToken(user.Id, DateTime.UtcNow.AddMinutes(10), userDTO);
                    string? refreshToken = await _jWTHelper.GenerateJWTRefreshToken(user.Id, DateTime.UtcNow.AddMonths(6));
                    await _jwtServices.InsertJWTToken(new JwtDTO()
                    {
                        user = user,
                        ExpiredDate = DateTime.UtcNow.AddMonths(6),
                        Token = refreshToken,
                    });
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Data = _mapper.Map<CredentialDTO>(user);
                    serviceResponse.Data.RefreshToken = refreshToken;
                    serviceResponse.Data.Token = token;
                }
                else
                {
                    throw new UnauthorizedAccessException("The user or password you entered is incorrect");
                }
                return serviceResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task verifyEmailAsync(string userid)
        {
            var u = _userRepository.GetById(int.Parse(userid));
            if (u == null || u.IsVerified == true)
            {
                return;
            }
            await _emailServies.sendActivationEmail(u);
        }

        public async Task<ServiceResponse<TokenDTO>> refreshTokenAsync(string reftoken)
        {
            var serviceResponse = new ServiceResponse<TokenDTO>();

            var claim = _jWTHelper.ValidateToken(reftoken);

            if (claim.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier))
            {
                var userid = claim.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                var user = _userRepository.GetById(int.Parse(userid));
                if (user == null)
                {
                    serviceResponse.ResponseType = EResponseType.Unauthorized;
                    serviceResponse.Message = "Could not found User from token.";
                }
                else
                {

                    var userDTO = _mapper.Map<UserModel, UserDTO>(user);

                    string? token = await _jWTHelper.GenerateJWTToken(user.Id, DateTime.UtcNow.AddMinutes(10), userDTO);

                    if (token == null)
                    {
                        serviceResponse.ResponseType = EResponseType.Unauthorized;
                        serviceResponse.Message = "Could not found User from token.";
                    }
                    else
                    {
                        TokenDTO _tokendto = new TokenDTO();
                        _tokendto.Token = token;
                        serviceResponse.Data = _tokendto;
                    }

                }
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.Unauthorized;
                serviceResponse.Message = "Could not found User from token.";
            }
            return serviceResponse;

        }
        public async Task<ServiceResponse<object>> activeEmailAsync(string Token)
        {
            var serviceResponse = new ServiceResponse<object>();
            try
            {
                var claim = _jWTHelper.ValidateToken(Token);
                var userid = claim.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                var action = claim.Claims.FirstOrDefault(c => c.Type == "action")!.Value;

                var user = _userRepository.GetById(int.Parse(userid));
                if (user == null || action == null || user.IsVerified == true)
                {
                    serviceResponse.ResponseType = EResponseType.Unauthorized;
                    serviceResponse.Message = "Could not found User or activated already.";
                    return serviceResponse;
                }
                if (action == "confirm")
                {
                    user.IsVerified = true;
                    _context.Update(user);
                    _context.SaveChanges();
                    serviceResponse.ResponseType = EResponseType.Success;
                    serviceResponse.Message = "Activate Success.";

                }
            }
            catch (Exception ex)
            {

            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<object>> sendForgotEmailVerify(string useremail)
        {
            var serviceResponse = new ServiceResponse<object>();
            try
            {
                var user = await _userRepository.getUserByEmail(useremail);
                if (user == null)
                {
                    serviceResponse.ResponseType = EResponseType.Unauthorized;
                    serviceResponse.Message = "Could not found user";
                }
                else
                {
                    await _emailServies.sendForgotPassword(user);
                }
            }
            catch (Exception e)
            {

            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> logout(string reftoken)
        {
           var serviceResponse = new ServiceResponse<object>();
            try { 
                await _jwtServices.InvalidateToken(reftoken);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Log out successful!";
                return serviceResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
