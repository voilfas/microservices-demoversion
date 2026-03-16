using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using product.Application;
using product.Domain.Models;

namespace product.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _dbContext;

    public ProductRepository(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
    }

    public async Task<IReadOnlyList<Product>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalAmountAsync()
    {
        return await _dbContext.Products.AsNoTracking().CountAsync(); 
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        
        if (product == null)
            return Result.Failure("Product not found");
        
        _dbContext.Products.Remove(product);
        
        return Result.Success();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }
} 