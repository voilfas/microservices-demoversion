namespace product.Application.UseCases.GetProducts;

public record DtoProductList(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity
);