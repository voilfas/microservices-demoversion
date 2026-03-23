using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using product.Application.Common;
using product.Application.Interfaces;

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

        var productsQuery = _dbContext.Products.AsNoTracking();
        
        if (query.MinPrice.HasValue)
            productsQuery = productsQuery.Where(p => p.Price >= query.MinPrice.Value);

        if (query.MaxPrice.HasValue)
            productsQuery = productsQuery.Where(p => p.Price <= query.MaxPrice.Value);
        
        if (!string.IsNullOrEmpty(query.NameProduct))
            productsQuery = productsQuery.Where(p => EF.Functions.Like(p.Name, $"%{query.NameProduct}%"));

        productsQuery = query.SortBy switch
        {
            ProductSortBy.Name => 
                query.SortDirection == SortDirection.Desc 
                    ? productsQuery.OrderByDescending(p => p.Name) 
                    : productsQuery.OrderBy(p => p.Name),
            
            ProductSortBy.Price =>
                query.SortDirection == SortDirection.Desc
                    ? productsQuery.OrderByDescending(p => p.Price)
                    : productsQuery.OrderBy(p => p.Price),
            
            ProductSortBy.Quantity =>
                query.SortDirection == SortDirection.Desc
                    ? productsQuery.OrderByDescending(p => p.Quantity)
                    : productsQuery.OrderBy(p => p.Quantity),
            
            ProductSortBy.UpdatedAt =>
                query.SortDirection == SortDirection.Desc
                    ? productsQuery.OrderByDescending(p => p.UpdatedAt)
                    : productsQuery.OrderBy(p => p.UpdatedAt),
            
            _ => query.SortDirection == SortDirection.Desc 
                ? productsQuery.OrderByDescending(p => p.CreatedAt) 
                : productsQuery.OrderBy(p => p.CreatedAt)
        };

        var totalCount = await productsQuery.CountAsync();
        
        var productsList = await productsQuery
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
        
        return Result.Success(new PagedResult<DtoProductsList>(
            productsList, 
            query.PageNumber, 
            query.PageSize, 
            totalCount));

    }
}