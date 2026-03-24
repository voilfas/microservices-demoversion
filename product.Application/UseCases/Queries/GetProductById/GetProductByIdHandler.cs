using System.Text.Json;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using product.Application.Interfaces;

namespace product.Application.UseCases.Queries.GetProductById;

public class GetProductByIdHandler
{
    private readonly IProductReadDbContext _dbContext;
    private readonly IDistributedCache _cache;
    private readonly ILogger<GetProductByIdHandler> _logger;

    public GetProductByIdHandler(IProductReadDbContext dbContext,  IDistributedCache cache, ILogger<GetProductByIdHandler> logger)
    {
        _dbContext = dbContext;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<DtoGetProductById>> Handle(GetProductByIdQuery query)
    {
        string cacheKey = $"product:{query.Id}";
        
        _logger.LogInformation("Start searching for product in Redis db with ID: {query.Id}", query.Id);
        
        var cachedProduct = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedProduct))
        {
            var productDto = JsonSerializer.Deserialize<DtoGetProductById>(cachedProduct);
            
            _logger.LogInformation("Found product with ID: {ProductId} in cache.", productDto?.Id);
            
            return Result.Success(productDto);
        }
        
        _logger.LogInformation("Start searching for product in PostgreSQL db with ID: {query.Id}", query.Id);
        
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
       {
           _logger.LogWarning("Product with ID: {ProductId} not found in PostgreSQL db.", query.Id);
           
           return Result.Failure<DtoGetProductById>("Product not found");
       }

       var options = new DistributedCacheEntryOptions
       {
           AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
       };
       
       var serializedProduct = JsonSerializer.Serialize(product);
       await _cache.SetStringAsync(cacheKey, serializedProduct, options);
       
       _logger.LogInformation("Product with ID: {ProductId} successfully founded in PostgreSQL and added in cache.", query.Id);
       
       return Result.Success(product);
    }
}