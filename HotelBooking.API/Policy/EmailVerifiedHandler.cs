
using HotelBooking.API.Policy.Requirement;
using HotelBooking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotelBooking.API.Policy
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

                    var user = _context.users.FirstOrDefault(x => x.Id == int.Parse(userid));
                    if (user.IsVerified)
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
