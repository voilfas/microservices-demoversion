using System.Text.Json;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using product.Application.Interfaces;

namespace product.Application.UseCases.Queries.GetProductById;

public class GetProductByIdHandler
{
    private readonly IProductReadDbContext _dbContext;
    private readonly IDistributedCache _cache;

    public GetProductByIdHandler(IProductReadDbContext dbContext,  IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<Result<DtoGetProductById>> Handle(GetProductByIdQuery query)
    {
        string cacheKey = $"product:{query.Id}";
        
        var cachedProduct = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedProduct))
        {
            var productDto = JsonSerializer.Deserialize<DtoGetProductById>(cachedProduct);
            return Result.Success(productDto);
        }
        
       var product =  await _dbContext.Products
           .AsNoTracking()
           .Where(p => p.Id == query.Id)
           .Select(p => new DtoGetProductById(
               p.Id,
               p.Name, 
               p.Price,
               p.Quantity, 
               p.CreatedAt))
           .FirstOrDefaultAsync();
       
       if (product is null)
           return Result.Failure<DtoGetProductById>("Product not found");

       var options = new DistributedCacheEntryOptions
       {
           AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
       };
       
       var serializedProduct = JsonSerializer.Serialize(product);
       await _cache.SetStringAsync(cacheKey, serializedProduct, options);
       
       return Result.Success(product);
    }
}