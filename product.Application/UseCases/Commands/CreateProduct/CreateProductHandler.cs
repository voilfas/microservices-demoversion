using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using product.Application.Interfaces;
using product.Domain.Models;

namespace product.Application.UseCases.Commands.CreateProduct;

public class CreateProductHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(IProductRepository productRepository,  ILogger<CreateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand command)
    {
        _logger.LogInformation("Start creating product. " +
                               "Name: {ProductName}, " +
                               "Price: {ProductPrice}, " +
                               "Quantity: {ProductQuantity}", command.Name, command.Price,  command.Quantity );
        
        var productResult = Product.Create(
            command.Name,
            command.Price,
            command.Quantity
        );

        if (productResult.IsFailure)
        {
            _logger.LogError("Could not create product. Reason: {Error}", productResult.Error);
            
            return Result.Failure<Guid>(productResult.Error);
        }

        await _productRepository.AddAsync(productResult.Value);
        await _productRepository.SaveChangesAsync();
        
        _logger.LogInformation("Product created successfully with ID: {ProductId}.", productResult.Value.Id);
        
        return Result.Success(productResult.Value.Id);
    }
}