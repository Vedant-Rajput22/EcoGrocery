using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.ToTable("Products");
        b.HasKey(p => p.Id);
        b.Property<byte[]>("RowVersion").IsRowVersion();
        b.Property(p => p.Name).IsRequired().HasMaxLength(120);
        b.HasIndex(p => p.Name);

        b.OwnsOne(p => p.Price, m => 
            {
            m.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            m.Property(x => x.Currency).HasColumnType("char(3)").IsUnicode(false);
            });

        b.Property(p => p.Label).HasConversion<int>();
        b.Property(p => p.CarbonGrams).HasColumnType("decimal(9,2)");
    }
}
