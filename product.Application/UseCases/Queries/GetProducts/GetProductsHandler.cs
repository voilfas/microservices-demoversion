using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using product.Application.Common;

namespace product.Application.UseCases.Queries.GetProducts;

public class GetProductsHandler
{
    private readonly IProductReadDbContext _dbContext;

    public GetProductsHandler(IProductReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PagedResult<DtoProductsList>>> Handle(GetProductsQuery query)
    {
        if (query.PageNumber <= 0)
            return Result.Failure<PagedResult<DtoProductsList>>(
                "Page number must be greater than 0!");
        
        if (query.PageSize is <= 0 or > 50)
            return Result.Failure<PagedResult<DtoProductsList>>(
                "Page size must be between 1 and 50!");

        var productsList = await _dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new DtoProductsList(
                p.Id,
                p.Name,
                p.Price,
                p.Quantity,
                p.CreatedAt
            ))
            .ToListAsync();

        var totalCount = await _dbContext.Products.CountAsync();
        
        return Result.Success(
            new PagedResult<DtoProductsList>(
                productsList, 
                query.PageNumber, 
                query.PageSize, 
                totalCount));

    }
}