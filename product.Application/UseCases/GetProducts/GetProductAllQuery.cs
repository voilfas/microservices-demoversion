namespace product.Application.UseCases.GetProducts;

public record GetProductAllQuery(
    int PageNumber,
    int PageSize
);