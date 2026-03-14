using product.Domain.Models;

namespace product.Application;

public interface IProductRepository
{
    Task SaveChangesAsync(); 
    Task AddAsync(Product product); 
    Task<IReadOnlyList<Product>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetTotalAmountAsync();
    Task<bool> DeleteAsync(Guid id); 
    Task<Product> GetByIdAsync(Guid id); 
}