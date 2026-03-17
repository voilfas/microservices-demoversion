using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace product.Application.UseCases.Queries.GetProductById;

public class GetProductByIdHandler
{
    private readonly IProductReadDbContext _dbContext;

    public GetProductByIdHandler(IProductReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<DtoGetProductById>> Handle(GetProductByIdQuery query)
    {
       var product =  await _dbContext.Products
           .AsNoTracking()
           .Where(p => p.Id == query.Id)
           .Select(p => new DtoGetProductById(
               p.Id,
               p.Name, 
               p.Price,
               p.Quantity, 
               p.CreatedAt))
           .FirstOrDefaultAsync();
       
       if (product is null)
           return Result.Failure<DtoGetProductById>("Product not found");
       
       return Result.Success(product);
    }
}