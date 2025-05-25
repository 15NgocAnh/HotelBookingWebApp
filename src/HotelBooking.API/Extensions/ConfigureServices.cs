using HotelBooking.Infrastructure.Authentication;
using HotelBooking.Infrastructure.Email;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace HotelBooking.API.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServicesConfig(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IJWTHelper, JWTHelper>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IEmailSender, EmailSenderServices>();

        return services;
    }
} 