using AutoMapper;
using HotelBooking.Application.Mappings;

namespace HotelBooking.API.Extensions;

public static class ConfigureAutoMapper
{
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddSingleton(provider => new MapperConfiguration(options =>
        {
            options.AddProfile(new AuthMappingProfile());
            options.AddProfile(new BookingMappingProfile());
            options.AddProfile(new InvoiceMappingProfile());
            options.AddProfile(new RoomTypeMappingProfile());
            options.AddProfile(new UserMappingProfile());
            options.AddProfile(new RoleMappingProfile());
            options.AddProfile(new HotelMappingProfile());
            options.AddProfile(new BuildingMappingProfile());
        }).CreateMapper());

        return services;
    }
} 