using product.Application.UseCases.Queries.GetProducts;

namespace product.Api.Dto;

public class GetProductsRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? NameProduct { get; set; }
    
    public ProductSortBy? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}