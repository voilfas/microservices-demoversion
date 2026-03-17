using Microsoft.EntityFrameworkCore;
using product.Domain.Models;

namespace product.Application;

public interface IProductReadDbContext
{
    DbSet<Product>  Products { get;}
}