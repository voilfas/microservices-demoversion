namespace product.Api.Dto;

public record UpdateProductResponce(
    string Name,
    decimal Price,
    int Quantity
);