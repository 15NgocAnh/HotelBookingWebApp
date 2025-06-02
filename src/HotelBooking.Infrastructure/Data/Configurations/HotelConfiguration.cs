using HotelBooking.Domain.AggregateModels.HotelAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Data.Configurations;
public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable(nameof(Hotel))
            .HasKey(n => n.Id);

        builder.Property(n => n.Name)
           .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
           .IsRequired();
    }
}
