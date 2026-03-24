using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using product.Application.Interfaces;

namespace product.Application.UseCases.Commands.UpdateProduct;

public class UpdateProductHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IDistributedCache _cache;
    private readonly ILogger<UpdateProductHandler> _logger;
    
    public UpdateProductHandler(IProductRepository productRepository, IDistributedCache cache,  ILogger<UpdateProductHandler> logger)
    {
        _productRepository = productRepository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<UpdateProductDto>> Handle(UpdateProductCommand command)
    {
        _logger.LogInformation("Start updating product. " +
                               "Name: {ProductName}, " +
                               "Price: {ProductPrice}, " +
                               "Quantity: {ProductQuantity}", command.Name, command.Price,  command.Quantity );
        
        var product = await _productRepository.GetByIdAsync(command.Id);
        
        if (product is null)
            return Result.Failure<UpdateProductDto>("Product not found!");

        var updateProduct = product.UpdateProduct(command.Name, command.Price, command.Quantity);

        if (updateProduct.IsFailure)
        {
            _logger.LogWarning("Could not update product. Reason: {Error}.",  updateProduct.Error);
            
            return Result.Failure<UpdateProductDto>(updateProduct.Error);
        }
        
        await _productRepository.SaveChangesAsync();
        
        await _cache.RemoveAsync($"product:{product.Id}");
        
        _logger.LogInformation("Product with ID: {ProductId} successfully updated in DB.", product.Id);

        var dto = new UpdateProductDto(
            product.Id, 
            product.Name, 
            product.Price, 
            product.Quantity, 
            product.CreatedAt, 
            product.UpdatedAt);
        
        return Result.Success(dto);
    }
}