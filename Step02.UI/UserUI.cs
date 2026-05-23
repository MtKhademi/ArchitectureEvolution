//using Step02.BLL.Services;
//using Step02.DAL.Entities;

//namespace Step02.UI;

//public static class UserUI
//{
//    private static IUserService _userService;

//    public static void Initialize(IUserService userService)
//    {
//        _userService = userService;
//    }

//    // ============================================================
//    // Seed Admin User
//    // ============================================================
//    public static void SeedData()
//    {
//        try
//        {
//            var user = _userService.Register("admin", "admin123", "admin123");
//            _userService.ChangeRole(user.Id, "Admin");
//            var admin = _userService.Login("admin", "admin123");
//            // دستی رول رو Admin می‌کنیم چون Register همیشه Customer می‌سازه
//            // توی برنامه واقعی این کار رو توی Service انجام می‌دیم
//        }
//        catch
//        {
//            // admin already exists
//        }
//    }

//    // ============================================================
//    // Login Screen
//    // ============================================================
//    public static User ShowLogin()
//    {
//        while (true)
//        {
//            Console.Clear();
//            Console.WriteLine("╔══════════════════════════════════════╗");
//            Console.WriteLine("║                OMS                   ║");
//            Console.WriteLine("╠══════════════════════════════════════╣");
//            Console.WriteLine("║                LOGIN                 ║");
//            Console.WriteLine("╠══════════════════════════════════════╣");
//            Console.WriteLine("║  1. Login                            ║");
//            Console.WriteLine("║  2. Register                         ║");
//            Console.WriteLine("║  3. Exit                             ║");
//            Console.WriteLine("╚══════════════════════════════════════╝");
//            Console.Write("   Select: ");

//            var choice = Console.ReadLine();

//            try
//            {
//                switch (choice)
//                {
//                    case "1":
//                        return HandleLogin();
//                    case "2":
//                        HandleRegister();
//                        break;
//                    case "3":
//                        Environment.Exit(0);
//                        break;
//                    default:
//                        Console.WriteLine("Invalid option!");
//                        Console.ReadKey();
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                ex.LogOnConsole();
//                Console.ReadKey();
//            }
//        }
//    }

//    private static User HandleLogin()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║            LOGIN                     ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        Console.Write("Username: ");
//        var username = Console.ReadLine();

//        Console.Write("Password: ");
//        var password = ReadPassword();

//        var user = _userService.Login(username, password);

//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine($"\n✅ Welcome, {user.Username}!");
//        Console.ResetColor();
//        Console.ReadKey();

//        return user;
//    }

//    private static void HandleRegister()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║           REGISTER                   ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        Console.Write("Username: ");
//        var username = Console.ReadLine();

//        Console.Write("Password: ");
//        var password = ReadPassword();

//        Console.Write("Confirm Password: ");
//        var confirmPassword = ReadPassword();

//        var user = _userService.Register(username, password, confirmPassword);

//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine($"\n✅ Registration successful! Welcome, {user.Username}!");
//        Console.WriteLine("   You can now login with your credentials.");
//        Console.ResetColor();
//        Console.ReadKey();
//    }

//    // ============================================================
//    // User Management Menu (Admin only)
//    // ============================================================
//    public static void ShowUserManagementMenu()
//    {
//        while (true)
//        {
//            Console.Clear();
//            Console.WriteLine("╔══════════════════════════════════════╗");
//            Console.WriteLine("║       USER MANAGEMENT                ║");
//            Console.WriteLine("╠══════════════════════════════════════╣");
//            Console.WriteLine("║  1. View All Users                   ║");
//            Console.WriteLine("║  2. View User Details                ║");
//            Console.WriteLine("║  3. Deactivate User                  ║");
//            Console.WriteLine("║  4. Activate User                    ║");
//            Console.WriteLine("║  5. Back                             ║");
//            Console.WriteLine("╚══════════════════════════════════════╝");
//            Console.Write("   Select: ");

//            var choice = Console.ReadLine();

//            try
//            {
//                switch (choice)
//                {
//                    case "1": ViewAllUsers(); break;
//                    case "2": ViewUserDetails(); break;
//                    case "3": DeactivateUser(); break;
//                    case "4": ActivateUser(); break;
//                    case "5": return;
//                    default:
//                        Console.WriteLine("Invalid option!");
//                        Console.ReadKey();
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                ex.LogOnConsole();
//                Console.ReadKey();
//            }
//        }
//    }

//    private static void ViewAllUsers()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║           ALL USERS                  ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        var users = _userService.GetAll();

//        if (users.Count == 0)
//        {
//            Console.WriteLine("\nNo users found.");
//            Console.ReadKey();
//            return;
//        }

//        Console.WriteLine(new string('-', 65));
//        Console.WriteLine($"{"ID",-5} {"Username",-20} {"Role",-12} {"Status",-10} {"Last Login",-15}");
//        Console.WriteLine(new string('-', 65));

//        foreach (var user in users)
//        {
//            var statusColor = user.IsActive ? ConsoleColor.Green : ConsoleColor.Red;

//            Console.Write($"{user.Id,-5} {user.Username,-20} {user.Role,-12} ");
//            Console.ForegroundColor = statusColor;
//            Console.Write($"{(user.IsActive ? "Active" : "Inactive"),-10}");
//            Console.ResetColor();
//            Console.WriteLine($" {user.LastLogin?.ToString("yyyy-MM-dd"),-15}");
//        }

//        Console.WriteLine(new string('-', 65));
//        Console.ReadKey();
//    }

//    private static void ViewUserDetails()
//    {
//        Console.Clear();
//        Console.Write("User ID: ");
//        if (!int.TryParse(Console.ReadLine(), out var id)) return;

//        var user = _userService.GetById(id);

//        Console.WriteLine($"\nUsername: {user.Username}");
//        Console.WriteLine($"Role: {user.Role}");
//        Console.WriteLine($"Status: {(user.IsActive ? "Active" : "Inactive")}");
//        Console.WriteLine($"Created: {user.CreatedAt}");
//        Console.WriteLine($"Last Login: {user.LastLogin}");

//        Console.ReadKey();
//    }

//    private static void DeactivateUser()
//    {
//        Console.Clear();
//        Console.Write("User ID to deactivate: ");
//        if (!int.TryParse(Console.ReadLine(), out var id)) return;

//        var user = _userService.Deactivate(id);

//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine($"✅ User '{user.Username}' deactivated.");
//        Console.ResetColor();
//        Console.ReadKey();
//    }

//    private static void ActivateUser()
//    {
//        Console.Clear();
//        Console.Write("User ID to activate: ");
//        if (!int.TryParse(Console.ReadLine(), out var id)) return;

//        var user = _userService.Activate(id);

//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine($"✅ User '{user.Username}' activated.");
//        Console.ResetColor();
//        Console.ReadKey();
//    }

//    // ============================================================
//    // Helper: Read Password with Mask
//    // ============================================================
//    private static string ReadPassword()
//    {
//        var password = "";
//        while (true)
//        {
//            var key = Console.ReadKey(true);
//            if (key.Key == ConsoleKey.Enter) break;

//            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
//            {
//                password = password[..^1];
//                Console.Write("\b \b");
//            }
//            else if (!char.IsControl(key.KeyChar))
//            {
//                password += key.KeyChar;
//                Console.Write("*");
//            }
//        }
//        Console.WriteLine();
//        return password;
//    }
//}