using CSharpFunctionalExtensions;
using product.Domain.Models;

namespace product.Application.UseCases.CreateProduct;

public class CreateProductCommand
{
    public string Name { get; }
    public decimal Price { get; }
    public int Quantity { get; }

    public CreateProductCommand(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}