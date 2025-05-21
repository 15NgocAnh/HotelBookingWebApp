using HotelBooking.Domain.AggregateModels.BookingAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable(nameof(Booking))
            .HasKey(n => n.Id);

        builder.HasIndex(h => h.RoomId);

        var guestNavigation = builder.Metadata.FindNavigation(nameof(Booking.Guests));
        guestNavigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        var extraUsageNavigation = builder.Metadata.FindNavigation(nameof(Booking.ExtraUsages));
        extraUsageNavigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
