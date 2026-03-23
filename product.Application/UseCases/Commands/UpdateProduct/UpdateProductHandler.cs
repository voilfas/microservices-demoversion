using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Distributed;
using product.Application.Interfaces;
using product.Domain.Models;

namespace product.Application.UseCases.Commands.UpdateProduct;

public class UpdateProductHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IDistributedCache _cache;
    
    public UpdateProductHandler(IProductRepository productRepository, IDistributedCache cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }

    public async Task<Result<UpdateProductDto>> Handle(UpdateProductCommand command)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);
        
        if (product is null)
            return Result.Failure<UpdateProductDto>("Product not found!");

        var updateProduct = product.UpdateProduct(command.Name, command.Price, command.Quantity);
        
        if (updateProduct.IsFailure)
            return Result.Failure<UpdateProductDto>(updateProduct.Error);
        
        await _productRepository.SaveChangesAsync();
        
        await _cache.RemoveAsync($"product:{product.Id}");

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