namespace product.Application.UseCases.GetProductById;

public record DtoGetProductById(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity
);
