using CSharpFunctionalExtensions;
using product.Domain.Models;

namespace product.Application.Interfaces;

public interface IProductRepository
{
    Task SaveChangesAsync(); 
    Task AddAsync(Product product); 
    Task<IReadOnlyList<Product>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetTotalAmountAsync();
    Task<Result> DeleteAsync(Guid id); 
    Task<Product?> GetByIdAsync(Guid id); 
}