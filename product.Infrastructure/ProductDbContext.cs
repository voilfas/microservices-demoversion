using Microsoft.EntityFrameworkCore;
using product.Application;
using product.Domain.Models;

namespace product.Infrastructure;

public class ProductDbContext : DbContext, IProductReadDbContext
{
    public DbSet<Product>  Products { get; set; }
    
    public ProductDbContext(DbContextOptions<ProductDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }
}