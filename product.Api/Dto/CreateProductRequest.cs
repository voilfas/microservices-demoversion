namespace product.Api.Dto;

public record CreateProductRequest(
    string Name,
    decimal Price,
    int Quantity
);