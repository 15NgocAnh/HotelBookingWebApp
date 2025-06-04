using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HotelBooking.Application.Services.User
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<int> UserHotelIds
        {
            get
            {
                var claims = _httpContextAccessor.HttpContext?.User?
                    .FindAll("HotelId")
                    .Select(c => int.TryParse(c.Value, out var id) ? id : (int?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id!.Value);

                return claims ?? Enumerable.Empty<int>();
            }
        }

        public int UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                {
                    return userId;
                }
                return 0;
            }
        }

        public string Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role).ToString();
            }
        }

    }
}
