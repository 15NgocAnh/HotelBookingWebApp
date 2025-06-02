using HotelBooking.Domain.AggregateModels.BuildingAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Data.Configurations;
public class FloorConfiguration : IEntityTypeConfiguration<Floor>
{
    public void Configure(EntityTypeBuilder<Floor> builder)
    {
        builder.ToTable(nameof(Floor))
            .HasKey(n => n.Id);

        builder.Property(n => n.Name)
           .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
           .IsRequired();
    }
}
