using HotelBooking.Domain.AggregateModels.BedTypeAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class BedTypeConfiguration : IEntityTypeConfiguration<BedType>
{
    public void Configure(EntityTypeBuilder<BedType> builder)
    {
        builder.ToTable(nameof(BedType))
            .HasKey(n => n.Id);

        builder.Property(n => n.Name)
           .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
           .IsRequired();
    }
}
