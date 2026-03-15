using CSharpFunctionalExtensions;

namespace product.Domain.Models;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() {}
    
    private Product(string name, decimal price, int quantity)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Quantity = quantity;
        CreatedAt = DateTime.UtcNow;
    }

    private static Result Validate(string name, decimal price, int quantity)
    {
        if (string.IsNullOrEmpty(name))
            return Result.Failure("Name can't be empty!");

        if (name.Length is < 2 or > 100)
            return Result.Failure("Name can't be less than 2 and more than 100 characters!");

        if (price <= 0)
            return Result.Failure("Price must be greater than 0!");
        
        return quantity < 0 ? Result.Failure("Quantity cannot be negative!") : Result.Success();
    }

    public static Result<Product> Create(string name, decimal price, int quantity)
    {
        var catalogAggregate = Validate(name, price, quantity);
        
        return catalogAggregate.IsFailure ? Result.Failure<Product>(catalogAggregate.Error) 
            : Result.Success(new Product(name, price, quantity));
    }
    
    public Result UpdateProduct(string name, decimal price, int quantity)
    {
        var catalogAggregateUpdate = Validate(name, price, quantity);
        
        if (catalogAggregateUpdate.IsFailure)
            return Result.Failure(catalogAggregateUpdate.Error);

        Name = name;
        Price = price;
        Quantity = quantity;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
    
    public Result IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure<Product>("Quantity cannot be negative!");
        
        Quantity += quantity;
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }
    
    public Result DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure("Quantity cannot be negative!");
        
        if(Quantity < quantity)
            return Result.Failure("Cannot decrease when Quantity is less than decrease quantity!");
        
        Quantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }

    public Result ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            return Result.Failure("Price must be greater than 0!");
        
        if (Price == newPrice)
            return Result.Failure("New price must be different from current price!");
        
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }
}