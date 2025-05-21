using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Application.Utils.Enum;
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
        private readonly JwtSettings _jwtSetting;

        public JWTHelper(IOptions<JwtSettings> jwtSetting)
        {
            _jwtSetting = jwtSetting.Value;
        }

        public async Task<string> GenerateJWTToken(int id, User user, string roleName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSetting.Key);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()), // ID của User
                new Claim(ClaimTypes.Email, user.Email), // Email
                new Claim("FirstName", user.FirstName), // Họ
                new Claim("LastName", user.LastName), // Tên
                new Claim(ClaimTypes.Role, roleName)
            }; 
            var token = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSetting.ExpireDays),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateJWTRefreshToken(int id, DateTime expire)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSetting.Key);
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

        public ClaimsPrincipal ValidateToken(string jwtToken, bool validateLifetime = true)
        {
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = validateLifetime,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key)),
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

            var key = Encoding.ASCII.GetBytes(_jwtSetting.Key);
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
