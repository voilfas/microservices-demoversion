namespace product.Api.Dto;

public record UpdateProductRequest(
    string Name,
    decimal Price,
    int Quantity
);