using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using product.Application.Interfaces;

namespace product.Application.UseCases.Commands.DeleteProduct;

public class DeleteProductHandler
{
    private readonly IProductRepository _repository;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(IProductRepository repository, ILogger<DeleteProductHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteProductCommand command)
    {
        _logger.LogInformation("Start deleting product. ID: {ProductId}.", command.Id);
        
        var productDelete = await _repository.DeleteAsync(command.Id);

        if (productDelete.IsFailure)
        {
            _logger.LogWarning("Could not delete product with ID: {ProductId}. Reason: {Error}.", command.Id, productDelete.Error);
            
            return Result.Failure("Product not found");
        }
        
        await _repository.SaveChangesAsync();
        
        _logger.LogInformation("Product with ID: {ProductId} successfully deleted from DB.", command.Id);
        
        return Result.Success();
    }
}