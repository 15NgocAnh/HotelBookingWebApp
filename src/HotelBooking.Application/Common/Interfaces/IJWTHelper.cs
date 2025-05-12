using HotelBooking.Domain.AggregateModels.UserAggregate;
using System.Security.Claims;

namespace HotelBooking.Application.Common.Interfaces
{

    public interface IJWTHelper
    {
        Task<string> GenerateJWTToken(int id, DateTime expire, User user);
        Task<string> GenerateJWTRefreshToken(int id, DateTime expire);
        Task<string> GenerateJWTMailAction(int id, DateTime expire, string action);
        ClaimsPrincipal ValidateToken(string jwtToken);
    }
}
