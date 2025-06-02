namespace HotelBooking.API.Extensions;

public static class ConfigureCors
{
    public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontendWithCredentials",
                builder =>
                {
                    builder.WithOrigins(configuration["Frontend"])
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .AllowAnyHeader();
                });
        });

        return services;
    }

    public static IApplicationBuilder UseCorsConfig(this IApplicationBuilder app)
    {
        app.UseCors("AllowFrontendWithCredentials");
        return app;
    }
} 