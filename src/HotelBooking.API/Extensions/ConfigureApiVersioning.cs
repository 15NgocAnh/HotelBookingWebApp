using Asp.Versioning;

namespace HotelBooking.API.Extensions;

public static class ConfigureApiVersioning
{
    public static IServiceCollection AddApiVersioningConfig(this IServiceCollection services)
    {
        var apiVersioningBuilder = services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.ReportApiVersions = true;
        });

        apiVersioningBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
} 