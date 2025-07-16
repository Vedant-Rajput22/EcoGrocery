using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data;

public class EcoDbContext
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>,
      IAppDbContext
{
    public EcoDbContext(DbContextOptions<EcoDbContext> opts) : base(opts) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public new DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var e in ChangeTracker.Entries<BaseAuditableEntity>()
                                        .Where(e => e.State == EntityState.Modified))
            e.Entity.Touch();

        return base.SaveChangesAsync(ct);
    }

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.ApplyConfigurationsFromAssembly(typeof(EcoDbContext).Assembly);
    }
}
