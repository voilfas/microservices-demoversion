using Microsoft.AspNetCore.Mvc;
using product.Api.Dto;
using product.Application.UseCases.Commands.CreateProduct;
using product.Application.UseCases.Commands.DeleteProduct;
using product.Application.UseCases.Commands.UpdateProduct;
using product.Application.UseCases.Queries.GetProductById;
using product.Application.UseCases.Queries.GetProducts;

namespace product.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly CreateProductHandler _createProductHandler;
    private readonly GetProductByIdHandler _getProductByIdHandler;
    private readonly UpdateProductHandler _updateProductHandler;
    private readonly DeleteProductHandler _deleteProductHandler;
    private readonly GetProductsHandler _getProductsHandler;
    
    public ProductController(
        CreateProductHandler createProductHandler,
        GetProductByIdHandler getProductByIdHandler, 
        UpdateProductHandler updateProductHandler, 
        DeleteProductHandler deleteProductHandler, 
        GetProductsHandler getProductsHandler)
    {
        _createProductHandler = createProductHandler;
        _getProductByIdHandler = getProductByIdHandler;
        _updateProductHandler = updateProductHandler;
        _deleteProductHandler = deleteProductHandler;
        _getProductsHandler = getProductsHandler;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var result = await _getProductByIdHandler.Handle(new GetProductByIdQuery(id));

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error,
                statusCode: StatusCodes.Status404NotFound,
                title: "Product not found"
                );
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductResponse response)
    {
        var command = new CreateProductCommand(response.Name, response.Price, response.Quantity);

        var result = await _createProductHandler.Handle(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Product not created");
        }
        
        return CreatedAtAction(
            nameof(GetProductById),
            new {id = result.Value},
            result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
    {
        var command = new UpdateProductCommand(id,  request.Name, request.Price, request.Quantity);
        
        var result = await _updateProductHandler.Handle(command);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Product not updated");
        }
        
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await _deleteProductHandler.Handle(new DeleteProductCommand(id));

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error,
                statusCode: StatusCodes.Status404NotFound,
                title: "Product not deleted");
        }
        
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsRequest request)
    {
        var query = new GetProductsQuery(
            PageNumber: request.PageNumber,
            PageSize: request.PageSize,
            MinPrice: request.MinPrice,
            MaxPrice: request.MaxPrice,
            NameProduct: request.NameProduct,
            SortBy: request.SortBy,
            SortDirection: request.SortDirection
        );

        var result = await _getProductsHandler.Handle(query);

        if (result.IsFailure)
        {
            return Problem(
                detail: result.Error,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Product not found");
        }

        return Ok(result.Value);
    }
}