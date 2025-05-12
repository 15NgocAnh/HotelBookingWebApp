using AutoMapper;
using HotelBooking.Application.Mappings;

namespace HotelBooking.API.Extensions;

public static class ConfigureAutoMapper
{
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddSingleton(provider => new MapperConfiguration(options =>
        {
            options.AddProfile(new BookingMappingProfile());
            options.AddProfile(new InvoiceMappingProfile());
        }).CreateMapper());

        return services;
    }
} 