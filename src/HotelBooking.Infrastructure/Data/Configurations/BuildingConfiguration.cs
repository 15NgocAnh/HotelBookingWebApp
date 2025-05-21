using HotelBooking.Domain.AggregateModels.BuildingAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class BuildingConfiguration : IEntityTypeConfiguration<Building>
{
    public void Configure(EntityTypeBuilder<Building> builder)
    {
        builder.ToTable(nameof(Building))
            .HasKey(n => n.Id);

        builder.HasIndex(h => h.HotelId);

        builder.Property(n => n.Name)
           .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
           .IsRequired();

        var floorNavigation = builder.Metadata.FindNavigation(nameof(Building.Floors));
        floorNavigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
