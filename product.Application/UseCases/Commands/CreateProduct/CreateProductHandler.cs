using CSharpFunctionalExtensions;
using product.Domain.Models;

namespace product.Application.UseCases.Commands.CreateProduct;

public class CreateProductHandler
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand command)
    {
        var productResult = Product.Create(
            command.Name,
            command.Price,
            command.Quantity
        );

        if (productResult.IsFailure)
            return Result.Failure<Guid>(productResult.Error);

        await _productRepository.AddAsync(productResult.Value);
        await _productRepository.SaveChangesAsync();
        
        return Result.Success(productResult.Value.Id);
    }
}