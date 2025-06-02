using Microsoft.AspNetCore.Http;

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
    }
}
