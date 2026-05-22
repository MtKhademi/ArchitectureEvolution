namespace Step017.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    private Product() //ORM
    {

    }


    //Factory method
    public static Product Create(string name, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.");

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");

        if (stockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.");


        return new Product
        {
            Name = name,
            Price = price,
            StockQuantity = stockQuantity
        };
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.");


        if (quantity > StockQuantity)
            throw new InvalidOperationException(
                $"Not enough stock. Available: {StockQuantity}, Requested: {quantity}");

        StockQuantity -= quantity;

    }
    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.");

        StockQuantity += quantity;
    }

    public void UodateInto(string newName, int newPrice)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty.");


        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative.");

        Name = newName.Trim();
        Price = newPrice;
    }

    public override string ToString()
        => $"Id: {Id} | {Name} | Price: {Price} | Stock Quantity: {StockQuantity}";
}
