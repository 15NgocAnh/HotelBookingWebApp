using FluentValidation;
using HotelBooking.Application.Behaviors;
using HotelBooking.Domain.Common;
using MediatR;
using System.Reflection;

namespace HotelBooking.API.Extensions;

public static class ConfigureMediatR
{
    public static IServiceCollection AddMediatRConfig(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });

        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
} 