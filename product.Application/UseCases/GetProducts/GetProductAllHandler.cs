using CSharpFunctionalExtensions;
using product.Application.Common;

namespace product.Application.UseCases.GetProducts;

public class GetProductAllHandler
{
    private readonly IProductRepository _repository;

    public GetProductAllHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResult<DtoProductList>>> Handle(GetProductAllQuery query)
    {
        if (query.PageNumber <= 0)
            return Result.Failure<PagedResult<DtoProductList>>("Page number cannot be zero or  negative!");
        
        if (query.PageSize is <= 0 or > 50)
            return Result.Failure<PagedResult<DtoProductList>>("Page size must be between 1 and 50!");
        
        var productList = await _repository.GetPagedAsync(query.PageNumber, query.PageSize);
        
        var totalAmount = await _repository.GetTotalAmountAsync();

        var dto = productList
            .Select(p => new DtoProductList(
                p.Id, 
                p.Name, 
                p.Price, 
                p.Quantity))
            .ToList();

        var pagedResult = new PagedResult<DtoProductList>(
            dto, 
            query.PageNumber, 
            query.PageSize, 
            totalAmount
            );
        
        return Result.Success(pagedResult);
    }
}