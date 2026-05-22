using Step02.Common;
using Step02.UI;


var DI = new DependencyContainer();

// ============================================================
// UI Initialization
// ============================================================
ProductUI.Initialize(DI.ProductService);
OrderUI.Initialize(DI.OrderService, DI.ProductService);
UserUI.Initialize(DI.UserService);

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