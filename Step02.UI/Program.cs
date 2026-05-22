using Step02.BLL.Repositorties;
using Step02.BLL.Services;
using Step02.DAL.Data;
using Step02.DAL.Repositories;
using Step02.UI;

// ============================================================
// Database
// ============================================================
var db = new DatabaseContext();

// ============================================================
// Repositories
// ============================================================
IProductRepository productRepo = new ProductRepository(db);
IOrderRepository orderRepo = new OrderRepository(db);
IUserRepository userRepo = new UserRepository(db);

// ============================================================
// Services
// ============================================================
IProductService realProductService = new ProductService(productRepo);
IProductService productService = new ProductLoggingService(realProductService);

IOrderService orderService = new OrderService(orderRepo, productRepo, userRepo);
//IOrderService orderService = new OrderLoggingService(realOrderService);

IUserService userService = new UserService(userRepo);
//IUserService userService = new UserLoggingService(realUserService);

// ============================================================
// UI Initialization
// ============================================================
ProductUI.Initialize(productService);
OrderUI.Initialize(orderService, productService);
UserUI.Initialize(userService);

// ============================================================
// Seed Data
// ============================================================
ProductUI.SeedData();
UserUI.SeedData();

// ============================================================
// Login
// ============================================================
var currentUser = UserUI.ShowLogin();


//--> Set current User
OrderUI._currentUserId = currentUser.Id;
OrderUI._currentUserRole = currentUser.Role;


ProductUI._currentUserId = currentUser.Id;
ProductUI._currentUserRole = currentUser.Role;

// ============================================================
// Main Loop
// ============================================================
while (true)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════╗");
    Console.WriteLine("║     ORDER MANAGEMENT SYSTEM          ║");
    Console.WriteLine("╠══════════════════════════════════════╣");
    Console.WriteLine($"║  User: {currentUser.Username,-28} ║");
    Console.WriteLine($"║  Role: {currentUser.Role,-28} ║");
    Console.WriteLine("╠══════════════════════════════════════╣");

    if (currentUser.Role == "Admin")
    {
        Console.WriteLine("║  1. Product Management              ║");
        Console.WriteLine("║  2. Order Management                ║");
        Console.WriteLine("║  3. User Management                 ║");
        Console.WriteLine("║  4. Logout                          ║");
        Console.WriteLine("║  5. Exit                            ║");
    }
    else
    {
        Console.WriteLine("║  1. Product Management              ║");
        Console.WriteLine("║  2. Order Management                ║");
        Console.WriteLine("║  3. Logout                          ║");
        Console.WriteLine("║  4. Exit                            ║");
    }

    Console.WriteLine("╚══════════════════════════════════════╝");
    Console.Write("   Select: ");

    var choice = Console.ReadLine();

    try
    {
        if (currentUser.Role == "Admin")
        {
            switch (choice)
            {
                case "1": ProductUI.ShowProductMenu(); break;
                case "2": OrderUI.ShowOrderMenu(); break;
                case "3": UserUI.ShowUserManagementMenu(); break;
                case "4": currentUser = UserUI.ShowLogin(); break;
                case "5": Console.WriteLine("Goodbye!"); return;
                default: Console.WriteLine("Invalid option!"); Console.ReadKey(); break;
            }
        }
        else
        {
            switch (choice)
            {
                case "1": ProductUI.ShowProductMenu(); break;
                case "2": OrderUI.ShowOrderMenu(); break;
                case "3": currentUser = UserUI.ShowLogin(); break;
                case "4": Console.WriteLine("Goodbye!"); return;
                default: Console.WriteLine("Invalid option!"); Console.ReadKey(); break;
            }
        }
    }
    catch (Exception ex)
    {
        ex.LogOnConsole();
        Console.ReadKey();
    }
}