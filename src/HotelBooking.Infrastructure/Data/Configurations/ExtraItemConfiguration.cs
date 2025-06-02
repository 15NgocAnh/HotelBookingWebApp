using HotelBooking.Domain.AggregateModels.ExtraItemAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Data.Configurations;
public class ExtraItemConfiguration : IEntityTypeConfiguration<ExtraItem>
{
    public void Configure(EntityTypeBuilder<ExtraItem> builder)
    {
        builder.ToTable(nameof(ExtraItem))
            .HasKey(n => n.Id);

        builder.HasIndex(h => h.ExtraCategoryId);

        builder.Property(n => n.Name)
           .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
           .IsRequired();

        builder.Property(n => n.Price)
            .HasPrecision(18, 2);
    }
}
