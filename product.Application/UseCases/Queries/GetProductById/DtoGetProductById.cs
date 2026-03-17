namespace product.Application.UseCases.Queries.GetProductById;

public record DtoGetProductById(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity,
    DateTime CreatedAt
);
