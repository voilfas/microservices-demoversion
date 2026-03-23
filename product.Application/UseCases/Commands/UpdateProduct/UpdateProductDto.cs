namespace product.Application.UseCases.Commands.UpdateProduct;

public record UpdateProductDto(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity,
    DateTime CreatedAt,
    DateTime? UpdatedAt);