namespace product.Api.Dto;

public record CreateProductResponce(
    string Name,
    decimal Price,
    int Quantity
);