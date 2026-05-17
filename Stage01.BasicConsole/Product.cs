namespace Stage01.BasicConsole;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public override string ToString()
        => $"Id: {Id} | {Name} | Price: {Price} | Stock Quantity: {StockQuantity}";
}
