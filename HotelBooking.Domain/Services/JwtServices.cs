using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.DTOs.User;
using HotelBooking.Domain.Encryption;
using HotelBooking.Domain.Repository.Interfaces;
using HotelBooking.Data;
using HotelBooking.Domain.Services.Interfaces;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Services
{
    public class JwtServices : IJwtServices
    {
        private readonly IMapper _mapper;
        private readonly IJwtRepository _jwtRepository;
        private readonly IPasswordHasher _hasher;

        public JwtServices(IMapper mapper, IJwtRepository jwtRepository, IPasswordHasher hasher)
        {
            _mapper = mapper;
            _jwtRepository = jwtRepository;
            _hasher = hasher;
        }

        public async Task InsertJWTToken(JwtDTO jwt)
        {
            var needToAdd = _mapper.Map<JWTModel>(jwt);
            if (needToAdd != null)
            {
                await _jwtRepository.AddAsync(needToAdd);
            }
            else
            {
                throw new Exception();
            }
        }
        public async Task<bool> IsJWTValid(string token)
        {

            return await _jwtRepository.FindByValue(_hasher.Hash(token)) != null;

        }
        public async Task InvalidateToken(string token)
        {
            try
            {
                await _jwtRepository.InvalidToken(_hasher.md5(token));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}