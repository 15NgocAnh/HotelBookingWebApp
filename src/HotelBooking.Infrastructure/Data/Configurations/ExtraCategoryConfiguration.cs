using HotelBooking.Domain.AggregateModels.ExtraCategoryAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class ExtraCategoryConfiguration : IEntityTypeConfiguration<ExtraCategory>
{
    public void Configure(EntityTypeBuilder<ExtraCategory> builder)
    {
        builder.ToTable(nameof(ExtraCategory))
            .HasKey(n => n.Id);

        builder.Property(n => n.Name)
           .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
           .IsRequired();
    }
}
