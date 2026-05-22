using Step02.BLL;

namespace Step02.UI;

internal static class ProductUI
{

    public static void SeedData(IProductService productService)
    {
        if (productService.GetAllProduct().Any()) return;

        productService.AddNewProduct(name: "Red Apple", 1.50m, 100);
        productService.AddNewProduct(name: "Banana Bunch", 0.80m, 150);
        productService.AddNewProduct(name: "Fresh Orange", 1.20m, 80);
        productService.AddNewProduct(name: "Green Grapes", 3.50m, 50);
        productService.AddNewProduct(name: "Strawberry Pack", 4.00m, 30);

    }

    public static void AddProduct(IProductService productService)
    {
        Console.Clear();
        Console.WriteLine("======== Add new a product ============");
        Console.Write("Please enter product name : ");

        var productName = Console.ReadLine();

        Console.Write("Please enter product price : ");
        if (!decimal.TryParse(Console.ReadLine(), out var productPrice))
        {
            Console.WriteLine("Invalid product price");
            return;
        }

        Console.Write("Please enter product stock quantity : ");
        if (!int.TryParse(Console.ReadLine(), out var productStockQuantity))
        {
            Console.WriteLine("Invalid product stock quantity .");
            return;
        }

        try
        {
            var product = productService.AddNewProduct(productName, productPrice, productStockQuantity);

            //Console.Clear();
            Console.WriteLine($"Product created successfully! : product id : {product.Id}");
        }
        catch (Exception ex)
        {
            ex.LogOnConsole();
        }
    }

    public static void ViewAllProducts(IProductService productService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("======== View all products ============");
            foreach (var p in productService.GetAllProduct().OrderByDescending(x => x.Id))
            {
                Console.WriteLine(p);
            }
            Console.WriteLine("===================================");
        }
        catch (Exception ex)
        {

            ex.LogOnConsole();
        }
    }

    public static void UpdateAProduct(IProductService productService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("======== Update a product ============");

            ViewAllProducts(productService);

            Console.WriteLine("==============================");
            Console.Write("Please enter product id : ");
            if (!int.TryParse(Console.ReadLine(), out var productId))
            {
                Console.WriteLine("Invalid id .");
                return;
            }
            var productToUpdate = productService.GetProductById(productId);

            Console.Write($"Please enter new name - ({productToUpdate.Name}) : ");
            var newName = Console.ReadLine();
            if (string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("invalid product name");
                return;
            }

            Console.Write($"Please enter new price - ({productToUpdate.Price}) : ");
            var price = decimal.Parse(Console.ReadLine());

            Console.Write($"Please enter new quantity - ({productToUpdate.StockQuantity}) : ");
            var quantity = int.Parse(Console.ReadLine());

            productToUpdate = productService.UpdateProduct(productId, newName, price, quantity);

            Console.Clear();
            Console.WriteLine("Successfully update product");
            Console.WriteLine(productToUpdate);
        }
        catch (Exception ex)
        {
            ex.LogOnConsole();
        }
    }

    public static void DeleteAProduct(IProductService productService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("======== Delete a product ============");

            ViewAllProducts(productService);

            Console.WriteLine("==============================");
            Console.Write("Please enter product id : ");
            var productId = int.Parse(Console.ReadLine());

            var productToDelete = productService.DeleteProduct(productId);

            Console.Clear();
            Console.WriteLine("Delete product successfully . ");
            Console.WriteLine($"Deleted product : {productToDelete}");
        }
        catch (Exception ex)
        {
            ex.LogOnConsole();
        }
    }
}
