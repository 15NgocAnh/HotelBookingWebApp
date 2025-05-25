using HotelBooking.API.Filters;
using System.Text.Json.Serialization;

namespace HotelBooking.API.Extensions;

public static class ConfigureControllers
{
    public static IServiceCollection AddControllersConfig(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilterAttribute>();
        })
        .AddJsonOptions(opt => 
        { 
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
        });

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        return services;
    }
} 