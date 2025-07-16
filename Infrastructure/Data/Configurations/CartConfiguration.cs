using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> b)
    {
        b.ToTable("Carts");
        b.HasKey(c => c.Id);

        b.Property(c => c.IsActive).HasDefaultValue(true);

        b.HasIndex(c => c.AppUserId)
         .IsUnique()
         .HasFilter("[IsActive] = 1");

        b.Ignore(c => c.SubTotal);

        b.HasMany(c => c.Items)
         .WithOne()
         .HasForeignKey(i => i.CartId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}
