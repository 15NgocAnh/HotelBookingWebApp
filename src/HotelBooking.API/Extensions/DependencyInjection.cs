namespace HotelBooking.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddMediatRConfig()
            .AddAutoMapperConfig()
            .AddApplicationServicesConfig()
            .AddRepositoriesConfig();

        return services;
    }
} 