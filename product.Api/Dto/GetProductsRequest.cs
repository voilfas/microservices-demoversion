namespace product.Api.Dto;

public record GetProductsRequest(
    int PageNumber = 1,
    int PageSize = 10
);