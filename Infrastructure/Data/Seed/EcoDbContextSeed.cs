using Domain.Entities;
using Domain.Enums;                   
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seed;

public static class EcoDbContextSeed
{
    public static async Task SeedAsync(EcoDbContext db,
                                       UserManager<AppUser> users,
                                       RoleManager<IdentityRole<Guid>> roles)
    {
        if (!await roles.RoleExistsAsync(BuiltInRoles.Admin))
            await roles.CreateAsync(new IdentityRole<Guid>(BuiltInRoles.Admin));

        if (!await roles.RoleExistsAsync(BuiltInRoles.Customer))
            await roles.CreateAsync(new IdentityRole<Guid>(BuiltInRoles.Customer));

        if (!await users.Users.AnyAsync())
        {
            var admin = new AppUser("admin@eco.local", "Admin", "User");
            await users.CreateAsync(admin, "Admin123$");
            await users.AddToRoleAsync(admin, BuiltInRoles.Admin);
        }

        if (!await db.Products.AnyAsync())
        {
            var p1 = new Product("Bamboo Toothbrush",
                                 new Money(149, "INR"), 100,
                                 12, EcoLabel.PlasticFree);

            var p2 = new Product("Organic Shampoo Bar",
                                 new Money(299, "INR"), 50,
                                 25, EcoLabel.Organic);

            db.Products.AddRange(p1, p2);
            await db.SaveChangesAsync();
        }
    }
}
