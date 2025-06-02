using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Email.Policy.Requirement;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HotelBooking.Infrastructure.Email.Policy
{

    public class EmailVerifiedHandler : AuthorizationHandler<EmailVerifiedRequirement>
    {
        private readonly AppDbContext _context;

        public EmailVerifiedHandler(AppDbContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailVerifiedRequirement requirement)
        {
            try
            {
                if (context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                {
                    string userid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    var user = _context.Users.FirstOrDefault(x => x.Id == int.Parse(userid));
                    context.Succeed(requirement);
                }
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }
    }
}
