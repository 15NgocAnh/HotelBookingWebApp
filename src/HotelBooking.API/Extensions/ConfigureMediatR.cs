using HotelBooking.Application.Behaviors;
using HotelBooking.Application.Common;
using HotelBooking.Domain.Common;

namespace HotelBooking.API.Extensions;

public static class ConfigureMediatR
{
    public static IServiceCollection AddMediatRConfig(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }); 

        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
} 