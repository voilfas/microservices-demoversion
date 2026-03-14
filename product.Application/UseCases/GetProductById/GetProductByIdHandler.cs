using CSharpFunctionalExtensions;

namespace product.Application.UseCases.GetProductById;

public class GetProductByIdHandler
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<DtoGetProductById>> Handle(GetProductByIdQuery query)
    {
        var product = await _productRepository.GetByIdAsync(query.Id);

        if (product is null)
            return Result.Failure<DtoGetProductById>("Product not found");

        var dto = new DtoGetProductById(
            product.Id,
            product.Name,
            product.Price,
            product.Quantity
        );
        
        return Result.Success(dto);
    }
}