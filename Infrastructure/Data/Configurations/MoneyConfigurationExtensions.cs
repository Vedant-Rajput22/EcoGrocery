using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public static class MoneyConfigurationExtensions
{
    public static void ConfigureMoney<T>(this OwnedNavigationBuilder<T, Money> m)
        where T : class
    {
        m.Property(p => p.Amount)
         .HasColumnType("decimal(18,2)");           

        m.Property(p => p.Currency)
         .HasColumnType("char(3)")
         .IsUnicode(false);
    }
}
