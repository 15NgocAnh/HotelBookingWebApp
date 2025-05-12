using HotelBooking.Infrastructure.Email.Policy;
using HotelBooking.Infrastructure.Email.Policy.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace HotelBooking.API.Extensions;

public static class ConfigureAuthorization
{
    public static IServiceCollection AddAuthorizationConfig(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("emailverified", 
                policy => policy.Requirements.Add(new EmailVerifiedRequirement()));
        });

        services.AddScoped<IAuthorizationHandler, EmailVerifiedHandler>();

        return services;
    }
} 