using Step02.DAL.Entities;

namespace Step02.UI;

public static class ProductUI
{
    private static IProductService _productService;

    public static int _currentUserId;
    public static string _currentUserRole;

    public static void Initialize(IProductService productService)
    {
        _productService = productService;
    }

    // ============================================================
    // Seed Data
    // ============================================================
    public static void SeedData()
    {
        if (_productService.GetAllProduct().Any()) return;

        _productService.AddNewProduct("Red Apple", 1.50m, 100);
        _productService.AddNewProduct("Banana Bunch", 0.80m, 150);
        _productService.AddNewProduct("Fresh Orange", 1.20m, 80);
        _productService.AddNewProduct("Green Grapes", 3.50m, 50);
        _productService.AddNewProduct("Strawberry Pack", 4.00m, 30);
        _productService.AddNewProduct("Whole Milk", 2.50m, 60);
        _productService.AddNewProduct("Bread Loaf", 1.80m, 40);
        _productService.AddNewProduct("Chicken Breast", 7.99m, 25);
        _productService.AddNewProduct("Pasta Pack", 1.50m, 70);
        _productService.AddNewProduct("Tomato Sauce", 2.20m, 45);
    }

    // ============================================================
    // منوی اصلی محصولات
    // ============================================================
    public static void ShowProductMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║       PRODUCT MANAGEMENT            ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║  1. View All Products               ║");

            if (_currentUserRole == "Admin")
            {
                Console.WriteLine("║  2. Add New Product                 ║");
                Console.WriteLine("║  3. Update Product                  ║");
                Console.WriteLine("║  4. Delete Product                  ║");
                Console.WriteLine("║  5. Back to Main Menu               ║");
            }
            else
            {
                Console.WriteLine("║  2. Back to Main Menu               ║");
            }

            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.Write("   Select: ");

            var choice = Console.ReadLine();

            try
            {
                if (_currentUserRole == "Admin")
                {
                    switch (choice)
                    {
                        case "1": ViewAllProducts(); break;
                        case "2": AddProduct(); break;
                        case "3": UpdateProduct(); break;
                        case "4": DeleteProduct(); break;
                        case "5": return;
                        default: InvalidOption(); break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case "1": ViewAllProducts(); break;
                        case "2": return;
                        default: InvalidOption(); break;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogOnConsole();
                Console.ReadKey();
            }
        }
    }

    // ============================================================
    // ۱. اضافه کردن محصول جدید
    // ============================================================
    private static void AddProduct()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║          ADD NEW PRODUCT             ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.Write("Product name: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("❌ Product name cannot be empty.");
            Console.ReadKey();
            return;
        }

        Console.Write("Price: ");
        if (!decimal.TryParse(Console.ReadLine(), out var price) || price < 0)
        {
            Console.WriteLine("❌ Invalid price. Must be a positive number.");
            Console.ReadKey();
            return;
        }

        Console.Write("Stock quantity: ");
        if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity < 0)
        {
            Console.WriteLine("❌ Invalid quantity. Must be a positive number.");
            Console.ReadKey();
            return;
        }

        var product = _productService.AddNewProduct(name, price, quantity);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✅ Product created successfully!");
        Console.WriteLine($"   ID: {product.Id} | Name: {product.Name} | Price: {product.Price:C} | Stock: {product.StockQuantity}");
        Console.ResetColor();

        Console.ReadKey();
    }

    // ============================================================
    // ۲. نمایش همه محصولات
    // ============================================================
    private static void ViewAllProducts()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║          ALL PRODUCTS                ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

        var products = _productService.GetAllProduct();

        if (products.Count == 0)
        {
            Console.WriteLine("\nNo products found. Add some first!");
            Console.ReadKey();
            return;
        }

        PrintProductsTable(products);

        Console.ReadKey();
    }

    // ============================================================
    // ۳. ویرایش محصول
    // ============================================================
    private static void UpdateProduct()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║          UPDATE PRODUCT              ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

        // نمایش محصولات برای انتخاب
        var products = _productService.GetAllProduct();
        PrintProductsTable(products);

        Console.Write("\nProduct ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var productId))
        {
            Console.WriteLine("❌ Invalid ID.");
            Console.ReadKey();
            return;
        }

        var existingProduct = _productService.GetProductById(productId);

        Console.WriteLine($"\nCurrent: {existingProduct.Name} | {existingProduct.Price:C} | Stock: {existingProduct.StockQuantity}");

        Console.Write($"New name ({existingProduct.Name}): ");
        var newName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newName))
            newName = existingProduct.Name;

        Console.Write($"New price ({existingProduct.Price}): ");
        var priceInput = Console.ReadLine();
        if (!decimal.TryParse(priceInput, out var newPrice) || newPrice < 0)
            newPrice = existingProduct.Price;

        Console.Write($"New stock quantity ({existingProduct.StockQuantity}): ");
        var qtyInput = Console.ReadLine();
        if (!int.TryParse(qtyInput, out var newQty) || newQty < 0)
            newQty = existingProduct.StockQuantity;

        var updated = _productService.UpdateProduct(productId, newName, newPrice, newQty);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✅ Product updated successfully!");
        Console.WriteLine($"   {updated.Name} | {updated.Price:C} | Stock: {updated.StockQuantity}");
        Console.ResetColor();

        Console.ReadKey();
    }

    // ============================================================
    // ۴. حذف محصول
    // ============================================================
    private static void DeleteProduct()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║          DELETE PRODUCT              ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

        var products = _productService.GetAllProduct();
        PrintProductsTable(products);

        Console.Write("\nProduct ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var productId))
        {
            Console.WriteLine("❌ Invalid ID.");
            Console.ReadKey();
            return;
        }

        var product = _productService.GetProductById(productId);

        Console.Write($"\n⚠️  Are you sure you want to delete '{product.Name}'? (y/n): ");
        var confirm = Console.ReadLine()?.ToLower();

        if (confirm != "y")
        {
            Console.WriteLine("Deletion cancelled.");
            Console.ReadKey();
            return;
        }

        _productService.DeleteProduct(productId);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ Product '{product.Name}' deleted successfully.");
        Console.ResetColor();

        Console.ReadKey();
    }

    // ============================================================
    // Helper: چاپ جدول محصولات
    // ============================================================
    private static void PrintProductsTable(List<Product> products)
    {
        Console.WriteLine(new string('-', 75));
        Console.WriteLine($"{"ID",-5} {"Name",-20} {"Price",-15} {"Stock",-10} {"Status",-10}");
        Console.WriteLine(new string('-', 75));

        foreach (var product in products.OrderByDescending(p => p.Id))
        {
            var statusText = product.StockQuantity switch
            {
                0 => "Out of Stock",
                <= 5 => "Low Stock",
                _ => "In Stock"
            };

            var statusColor = product.StockQuantity switch
            {
                0 => ConsoleColor.Red,
                <= 5 => ConsoleColor.Yellow,
                _ => ConsoleColor.Green
            };

            Console.Write($"{product.Id,-5} {product.Name,-20} {product.Price,13:C} {product.StockQuantity,8}  ");
            Console.ForegroundColor = statusColor;
            Console.Write($"{statusText,-10}");
            Console.ResetColor();
            Console.WriteLine();
        }

        Console.WriteLine(new string('-', 75));
        Console.WriteLine($"  Total: {products.Count} products");
    }

    // ============================================================
    // InvalidOption - پیام مشترک برای گزینه نامعتبر
    // ============================================================
    private static void InvalidOption()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("⚠️  Invalid option! Please try again.");
        Console.ResetColor();
        Thread.Sleep(1000);
    }
}