using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Lots;
using Microsoft.EntityFrameworkCore;

namespace ErpEssentials.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Lot> Lots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

