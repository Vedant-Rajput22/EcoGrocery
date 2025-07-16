using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> b)
    {
        b.ToTable("Orders");
        b.HasKey(o => o.Id);

        b.Property(o => o.Status).HasConversion<string>();

        b.OwnsOne(o => o.ShippingAddress);
        b.OwnsOne(o => o.ShippingFee, m => m.ConfigureMoney());
        b.OwnsOne(o => o.SubTotal, m => m.ConfigureMoney());

        b.Ignore(o => o.Total);
    }
}
