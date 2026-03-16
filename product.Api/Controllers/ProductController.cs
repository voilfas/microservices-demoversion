using Microsoft.AspNetCore.Mvc;
using product.Api.Dto;
using product.Application.UseCases.CreateProduct;
using product.Application.UseCases.DeleteProduct;
using product.Application.UseCases.GetProductById;
using product.Application.UseCases.GetProducts;
using product.Application.UseCases.UpdateProduct;

namespace product.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly CreateProductHandler _createProductHandler;
    private readonly GetProductByIdHandler _getProductByIdHandler;
    private readonly UpdateProductHandler _updateProductHandler;
    private readonly DeleteProductHandler _deleteProductHandler;
    private readonly GetProductAllHandler _getProductAllHandler;
    
    public ProductController(
        CreateProductHandler createProductHandler,
        GetProductByIdHandler getProductByIdHandler, 
        UpdateProductHandler updateProductHandler, 
        DeleteProductHandler deleteProductHandler, 
        GetProductAllHandler getProductAllHandler)
    {
        _createProductHandler = createProductHandler;
        _getProductByIdHandler = getProductByIdHandler;
        _updateProductHandler = updateProductHandler;
        _deleteProductHandler = deleteProductHandler;
        _getProductAllHandler = getProductAllHandler;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        
        var result = await _getProductByIdHandler.Handle(query);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var command = new CreateProductCommand(request.Name, request.Price, request.Quantity);

        var result = await _createProductHandler.Handle(command);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
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
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await _deleteProductHandler.Handle(new DeleteProductCommand(id));
        
        if(result.IsFailure)
            return NotFound(result.Error);
        
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsRequest request)
    {
        var result = await _getProductAllHandler
            .Handle(
                new GetProductAllQuery(
                    request.PageNumber, 
                    request.PageSize)
            );
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}