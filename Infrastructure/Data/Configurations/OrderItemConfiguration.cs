using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> b)
    {
        b.ToTable("OrderItems");
        b.HasKey(oi => oi.Id);

        b.OwnsOne(oi => oi.UnitPrice, m =>
 {
            m.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            m.Property(x => x.Currency).HasColumnType("char(3)").IsUnicode(false);
             });
    }
}
