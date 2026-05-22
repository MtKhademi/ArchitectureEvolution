using Step02.BLL.Repositorties;
using Step02.BLL.Services;
using Step02.DAL.Data;
using Step02.DAL.Repositories;
using Step02.UI;

// Database
var db = new DatabaseContext();

// Repositories
IProductRepository productRepo = new ProductRepository(db);
IOrderRepository orderRepo = new OrderRepository(db);

// Services
IProductService realProductService = new ProductService(productRepo);
IProductService productService = new ProductLoggingService(realProductService);

IOrderService orderService = new OrderService(orderRepo, productRepo);
//IOrderService orderService = new OrderLoggingService(realOrderService);

// UI
ProductUI.Initialize(productService);
OrderUI.Initialize(orderService, productService);

ProductUI.SeedData();

// Main Loop
while (true)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════╗");
    Console.WriteLine("║     ORDER MANAGEMENT SYSTEM          ║");
    Console.WriteLine("╠══════════════════════════════════════╣");
    Console.WriteLine("║  1. Product Management               ║");
    Console.WriteLine("║  2. Order Management                 ║");
    Console.WriteLine("║  3. Exit                             ║");
    Console.WriteLine("╚══════════════════════════════════════╝");
    Console.Write("   Select: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                ProductUI.ShowProductMenu();
                break;
            case "2":
                OrderUI.ShowOrderMenu();
                break;
            case "3":
                Console.WriteLine("Goodbye!");
                return;
            default:
                Console.WriteLine("Invalid option!");
                Console.ReadKey();
                break;
        }
    }
    catch (Exception ex)
    {
        ex.LogOnConsole();
        Console.ReadKey();
    }
}