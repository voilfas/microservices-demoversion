namespace product.Api.Dto;

public record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int Quantity,
    DateTime CreatedAt,
    DateTime? UpdatedAt
    );