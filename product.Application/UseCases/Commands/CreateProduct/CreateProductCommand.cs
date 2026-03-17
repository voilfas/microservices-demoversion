namespace product.Application.UseCases.Commands.CreateProduct;

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