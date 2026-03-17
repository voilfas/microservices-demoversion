namespace product.Application.UseCases.Queries.GetProducts;

public record GetProductsQuery(
    int PageNumber,
    int PageSize
);