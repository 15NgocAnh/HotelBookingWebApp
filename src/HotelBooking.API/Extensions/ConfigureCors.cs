namespace HotelBooking.API.Extensions;

public static class ConfigureCors
{
    public static IServiceCollection AddCorsConfig(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        return services;
    }

    public static IApplicationBuilder UseCorsConfig(this IApplicationBuilder app)
    {
        app.UseCors("AllowAllOrigins");
        return app;
    }
} 