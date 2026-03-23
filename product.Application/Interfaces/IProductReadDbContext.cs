using Microsoft.EntityFrameworkCore;
using product.Domain.Models;

namespace product.Application.Interfaces;

public interface IProductReadDbContext
{
    DbSet<Product>  Products { get;}
}