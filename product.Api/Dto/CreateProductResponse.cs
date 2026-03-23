namespace product.Api.Dto;

public record CreateProductResponse(
    string Name,
    decimal Price,
    int Quantity
);