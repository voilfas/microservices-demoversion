using CSharpFunctionalExtensions;
using product.Domain.Models;

namespace product.Application.UseCases.Commands.UpdateProduct;

public class UpdateProductHandler
{
    private readonly IProductRepository _productRepository;
    
    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Product>> Handle(UpdateProductCommand command)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);
        
        if (product is null)
            return Result.Failure<Product>("Product not found!");

        var updateProduct = product.UpdateProduct(command.Name, command.Price, command.Quantity);
        
        if (updateProduct.IsFailure)
            return Result.Failure<Product>(updateProduct.Error);

        await _productRepository.SaveChangesAsync();
        
        return Result.Success(product);
    }
}