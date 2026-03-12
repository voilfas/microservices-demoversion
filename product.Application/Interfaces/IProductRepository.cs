using product.Domain.Models;

namespace product.Application;

public interface IProductRepository
{
    Task SaveChangesAsync(); 
    Task AddAsync(Product product); 
    Task<IEnumerable<Product>> GetAllAsync(); 
    Task<bool> DeleteAsync(Guid id); 
    Task<Product> GetByIdAsync(Guid id); 
}