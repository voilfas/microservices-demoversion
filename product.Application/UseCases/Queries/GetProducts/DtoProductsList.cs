namespace product.Application.UseCases.Queries.GetProducts;

public record DtoProductsList(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity
);