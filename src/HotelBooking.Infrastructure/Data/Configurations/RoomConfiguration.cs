using HotelBooking.Domain.AggregateModels.RoomAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Data.Configurations;
public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable(nameof(Room))
            .HasKey(n => n.Id);

        builder.HasIndex(h => h.FloorId);

        builder.HasIndex(h => h.RoomTypeId);

        builder.Property(n => n.Name)
           .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
           .IsRequired();
    }
}
