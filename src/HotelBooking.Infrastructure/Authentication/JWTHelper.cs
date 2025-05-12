using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Domain.AggregateModels.UserAggregate;
using HotelBooking.Infrastructure.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelBooking.Infrastructure.Authentication
{
    public class JWTHelper : IJWTHelper
    {
        private readonly TokenSettings _tokenSetting;

        public JWTHelper(IOptions<TokenSettings> tokenSetting)
        {

            _tokenSetting = tokenSetting.Value;
        }

        public async Task<string> GenerateJWTToken(int id, DateTime expire, User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_tokenSetting.Secret);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()), // ID của User
                new Claim(ClaimTypes.Email, user.Email), // Email
                new Claim("FirstName", user.FirstName), // Họ
                new Claim("LastName", user.LastName) // Tên
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expire,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateJWTRefreshToken(int id, DateTime expire)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_tokenSetting.Secret);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: expire,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string jwtToken)
        {
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSetting.Secret)),
                };

                var principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                return new ClaimsPrincipal();
            }
        }

        public async Task<string> GenerateJWTMailAction(int id, DateTime expire, string action)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_tokenSetting.Secret);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim("action", action)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: expire,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            return tokenHandler.WriteToken(token);
        }

        public static bool JwtExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            return jsonToken == null || jsonToken.ValidTo < DateTime.UtcNow;
        }
    }
}
