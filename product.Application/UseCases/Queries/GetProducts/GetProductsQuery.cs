namespace product.Application.UseCases.Queries.GetProducts;

public record GetProductsQuery(
    int PageNumber,
    int PageSize,
    
    decimal? MinPrice,
    decimal? MaxPrice,
    string? NameProduct,
    
    ProductSortBy? SortBy,
    SortDirection? SortDirection
);