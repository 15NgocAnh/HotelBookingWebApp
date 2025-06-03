using HotelBooking.Domain.AggregateModels.InvoiceAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable(nameof(Invoice))
            .HasKey(n => n.Id);

        builder.HasIndex(h => h.BookingId);

        var itemNavigation = builder.Metadata.FindNavigation(nameof(Invoice.Items));
        itemNavigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        var paymentNavigation = builder.Metadata.FindNavigation(nameof(Invoice.Payments));
        paymentNavigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
