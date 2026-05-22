using Step02.DAL.Entities;
using System.Diagnostics;

public class ProductLoggingService : IProductService
{
    private readonly IProductService _inner;

    public ProductLoggingService(IProductService inner)
    {
        _inner = inner;
    }

    public Product AddNewProduct(string name, decimal price, int quantity)
    {
        // قبل از عملیات
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("┌─────────────────────────────────────┐");
        Console.WriteLine($"│ 🛒 Adding New Product                │");
        Console.WriteLine($"│ Name: {name,-30} │");
        Console.WriteLine($"│ Price: {price,28:C} │");
        Console.WriteLine($"│ Quantity: {quantity,25} │");
        Console.WriteLine("└─────────────────────────────────────┘");
        Console.ResetColor();

        // عملیات اصلی
        var result = _inner.AddNewProduct(name, price, quantity);

        // بعد از عملیات
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ Product '{result.Name}' added successfully! (ID: {result.Id})");
        Console.ResetColor();

        return result;
    }

    public Product DeleteProduct(int productId)
    {
        throw new NotImplementedException();
    }

    public List<Product> GetAllProduct()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("📋 Fetching all products...");
        Console.ResetColor();

        var stopwatch = Stopwatch.StartNew();
        var products = _inner.GetAllProduct();
        stopwatch.Stop();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ Found {products.Count} products in {stopwatch.ElapsedMilliseconds}ms");
        Console.ResetColor();

        return products;
    }

    public Product GetProductById(int productId)
    {
        //Console.ForegroundColor = ConsoleColor.Cyan;
        //Console.WriteLine($"🔍 Searching for product ID: {productId}");
        //Console.ResetColor();

        var product = _inner.GetProductById(productId);

        //Console.ForegroundColor = ConsoleColor.Green;
        //Console.WriteLine($"✅ Found: {product.Name} | Price: {product.Price:C}");
        //Console.ResetColor();

        return product;
    }

    public Product UpdateProduct(int productId, string name, decimal price, int quantity)
    {
        throw new NotImplementedException();
    }

    // و متدهای دیگه...
}