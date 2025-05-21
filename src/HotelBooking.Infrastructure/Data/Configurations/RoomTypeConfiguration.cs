using HotelBooking.Domain.AggregateModels.RoomTypeAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable(nameof(RoomType))
            .HasKey(n => n.Id);

        builder.Property(n => n.Name)
           .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
           .IsRequired();

        builder.Property(n => n.Price)
            .HasPrecision(18, 2);

        var bedTypeSetupDetailNavigation = builder.Metadata.FindNavigation(nameof(RoomType.BedTypeSetupDetails));
        bedTypeSetupDetailNavigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        var amenitySetupDetailNavigation = builder.Metadata.FindNavigation(nameof(RoomType.AmenitySetupDetails));
        amenitySetupDetailNavigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
