using HotelBooking.Application.Services.User;
using HotelBooking.Infrastructure.Authentication;
using HotelBooking.Infrastructure.Email;

namespace HotelBooking.API.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServicesConfig(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IJWTHelper, JWTHelper>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IEmailService, EmailSenderServices>();

        return services;
    }
} 