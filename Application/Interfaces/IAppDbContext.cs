using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartItems { get; }
    DbSet<Order> Orders { get; }
    DbSet<ProductImage> ProductImages { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
