using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Repositories;

namespace HotelBooking.API.Extensions;

public static class ConfigureRepositories
{
    public static IServiceCollection AddRepositoriesConfig(this IServiceCollection services)
    {
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IRoomRepository, RoomRepository>();
        services.AddTransient<IRoomTypeRepository, RoomTypeRepository>();
        services.AddTransient<IBookingRepository, BookingRepository>();
        services.AddTransient<IAmenityRepository, AmenityRepository>();
        services.AddTransient<IBedTypeRepository, BedTypeRepository>();
        services.AddTransient<IBuildingRepository, BuildingRepository>();
        services.AddTransient<IExtraCategoryRepository, ExtraCategoryRepository>();
        services.AddTransient<IExtraItemRepository, ExtraItemRepository>();
        services.AddTransient<IInvoiceRepository, InvoiceRepository>();
        services.AddTransient<IHotelRepository, HotelRepository>();
        services.AddTransient<IUserHotelRepository, UserHotelRepository>();

        services.AddScoped<IUnitOfWorkWithTransaction, EfUnitOfWork>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
} 