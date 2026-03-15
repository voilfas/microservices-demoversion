using CSharpFunctionalExtensions;

namespace product.Application.UseCases.DeleteProduct;

public class DeleteProductHandler
{
    private readonly IProductRepository _repository;

    public DeleteProductHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteProductCommand command)
    {
        var productDelete = await _repository.GetByIdAsync(command.Id);

        if (productDelete is null)
            return Result.Failure("Product not found");
        
        await _repository.SaveChangesAsync();
        return Result.Success();
    }
}