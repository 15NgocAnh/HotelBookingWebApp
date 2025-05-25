using HotelBooking.Domain.AggregateModels.AmenityAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Data.Configurations;
public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.ToTable(nameof(Amenity))
            .HasKey(n => n.Id);

        builder.Property(n => n.Name)
           .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
           .IsRequired();
    }
}
