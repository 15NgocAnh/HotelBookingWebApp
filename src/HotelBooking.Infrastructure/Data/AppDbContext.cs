using HotelBooking.Domain.AggregateModels.AmenityAggregate;
using HotelBooking.Domain.AggregateModels.BedTypeAggregate;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.AggregateModels.BuildingAggregate;
using HotelBooking.Domain.AggregateModels.ExtraCategoryAggregate;
using HotelBooking.Domain.AggregateModels.ExtraItemAggregate;
using HotelBooking.Domain.AggregateModels.HotelAggregate;
using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using HotelBooking.Domain.AggregateModels.RoomAggregate;
using HotelBooking.Domain.AggregateModels.RoomTypeAggregate;
using HotelBooking.Domain.AggregateModels.UserAggregate;
using HotelBooking.Domain.Common;
using Infrastructure.Data.Configurations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;
using System.Security.Claims;

namespace HotelBooking.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IMediator _mediator;
    
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<BedType> BedTypes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Building> Buildings { get; set; }
    public DbSet<ExtraCategory> ExtraCategories { get; set; }
    public DbSet<ExtraItem> ExtraItems { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserHotel> UserHotels { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public AppDbContext(
        IHttpContextAccessor httpContextAccessor, 
        DbContextOptions<AppDbContext> options, 
        IDomainEventDispatcher domainEventDispatcher,
        IMediator mediator) 
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _domainEventDispatcher = domainEventDispatcher;
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BuildingConfiguration).Assembly);

        // Configure owned entities
        modelBuilder.Entity<Booking>().OwnsMany(b => b.ExtraUsages);
        modelBuilder.Entity<Booking>().OwnsMany(b => b.Guests);
        modelBuilder.Entity<Invoice>().OwnsMany(b => b.Items);
        modelBuilder.Entity<RoomType>().OwnsMany(b => b.AmenitySetupDetails);
        modelBuilder.Entity<RoomType>().OwnsMany(b => b.BedTypeSetupDetails);
        //modelBuilder.Entity<Building>().OwnsMany(b => b.Floors);
        modelBuilder.Entity<Booking>().OwnsMany(b => b.Payments);
        modelBuilder.Entity<Invoice>().OwnsMany(b => b.Payments);

        // Configure value converters for enums
        modelBuilder.Entity<Booking>()
            .Property(b => b.Status)
            .HasConversion(new EnumToStringConverter<BookingStatus>());

        modelBuilder.Entity<Invoice>()
            .Property(i => i.Status)
            .HasConversion(new EnumToStringConverter<InvoiceStatus>());

        modelBuilder.Entity<Room>()
            .Property(r => r.Status)
            .HasConversion(new EnumToStringConverter<RoomStatus>());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update audit fields
        UpdateAuditFields();
        
        // Save changes to the database
        var result = await base.SaveChangesAsync(cancellationToken);
        
        // Dispatch domain events
        await DispatchDomainEventsAsync();
        
        return result;
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.SetUpdatedAt();
                entry.Entity.UpdatedBy = userId;
            }
        }
    }

    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        await _domainEventDispatcher.DispatchAndClearEvents(domainEntities);
    }
}