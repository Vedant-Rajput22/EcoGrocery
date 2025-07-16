using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> b)
    {
        b.ToTable("CartItems");
        b.HasKey(ci => ci.Id);

        b.OwnsOne(ci => ci.UnitPrice, m =>{
            m.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            m.Property(x => x.Currency).HasColumnType("char(3)").IsUnicode(false);
             });

        b.HasIndex(ci => new { ci.CartId, ci.ProductId }).IsUnique();
    }
}
