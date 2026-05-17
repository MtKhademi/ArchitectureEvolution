# Stage 02: Layered Architecture - N-Tier with EF Core & JWT Auth

## 🎯 Objective
Transform the single-file console app into a **Layered Architecture (N-Tier)** with proper separation of concerns. This stage introduces professional project structure, Entity Framework Core, SQL Server, and JWT authentication.

**Core Principle:** "Separate concerns, not just code."

## 📋 Prerequisites
- Completed Stage 01-B (Console App with Auth)
- SQL Server installed (Express or Developer edition)
- Understanding of C# interfaces and dependency injection
- Basic knowledge of HTTP and REST concepts

## 🏗️ Architectural Overview
We move from a single-file monolith to a **4-Layer Architecture**:

    ┌─────────────────────────────────────────────┐
    │                  UI Layer                    │
    │              (Console / Web API)              │
    ├─────────────────────────────────────────────┤
    │              Business Logic Layer            │
    │       (Services, Validators, DTOs)           │
    ├─────────────────────────────────────────────┤
    │            Data Access Layer                 │
    │      (Repositories, EF Core, DbContext)      │
    ├─────────────────────────────────────────────┤
    │                 Database                     │
    │              (SQL Server)                    │
    └─────────────────────────────────────────────┘

### Project Structure

    Stage-02-Layered-Architecture/
    ├── src/
    │   ├── Stage02.UI/                    # Presentation Layer
    │   │   ├── Program.cs
    │   │   ├── appsettings.json
    │   │   └── Stage02.UI.csproj
    │   │
    │   ├── Stage02.BLL/                   # Business Logic Layer
    │   │   ├── Services/
    │   │   │   ├── IAuthService.cs
    │   │   │   ├── AuthService.cs
    │   │   │   ├── IProductService.cs
    │   │   │   ├── ProductService.cs
    │   │   │   ├── IOrderService.cs
    │   │   │   ├── OrderService.cs
    │   │   │   ├── IUserService.cs
    │   │   │   └── UserService.cs
    │   │   ├── DTOs/
    │   │   │   ├── LoginDto.cs
    │   │   │   ├── RegisterDto.cs
    │   │   │   ├── ProductDto.cs
    │   │   │   └── OrderDto.cs
    │   │   └── Stage02.BLL.csproj
    │   │
    │   ├── Stage02.DAL/                   # Data Access Layer
    │   │   ├── Data/
    │   │   │   └── AppDbContext.cs
    │   │   ├── Repositories/
    │   │   │   ├── IUserRepository.cs
    │   │   │   ├── UserRepository.cs
    │   │   │   ├── IProductRepository.cs
    │   │   │   ├── ProductRepository.cs
    │   │   │   ├── IOrderRepository.cs
    │   │   │   └── OrderRepository.cs
    │   │   ├── Entities/
    │   │   │   ├── User.cs
    │   │   │   ├── Product.cs
    │   │   │   ├── Order.cs
    │   │   │   └── OrderItem.cs
    │   │   └── Stage02.DAL.csproj
    │   │
    │   └── Stage02.Common/               # Shared Utilities
    │       ├── Helpers/
    │       │   └── PasswordHasher.cs
    │       └── Stage02.Common.csproj
    │
    └── Stage02.sln

### Layer Communication Rulesobile
    UI ──────→ BLL ──────→ DAL ──────→ Database

    Rules:
    1. Each layer only depends on the layer directly below it
    2. UI never talks to DAL directly
    3. BLL depends on DAL abstractions (interfaces), not implementations
    4. Entities live in DAL and flow upward

## 💻 Implementation Strategy

### 1. Domain Entities (DAL)

    // Stage02.DAL/Entities/User.cs
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    // Stage02.DAL/Entities/Product.cs
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    // Stage02.DAL/Entities/Order.cs
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }

    // Stage02.DAL/Entities/OrderItem.cs
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // Stage02.DAL/Entities/Enums.cs
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }

### 2. Database Context (DAL)

    // Stage02.DAL/Data/AppDbContext.cs
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);
        }
    }

### 3. Repository Interfaces (DAL)

    // Stage02.DAL/Repositories/IUserRepository.cs
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<List<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> UsernameExistsAsync(string username);
    }

    // Stage02.DAL/Repositories/IProductRepository.cs
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> IsProductInOrderAsync(int productId);
    }

    // Stage02.DAL/Repositories/IOrderRepository.cs
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetByUserIdAsync(int userId);
        Task<Order> GetByIdAsync(int id);
        Task<Order> AddAsync(Order order);
    }

### 4. Repository Implementations (DAL)

    // Stage02.DAL/Repositories/UserRepository.cs
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username);
        }
    }

    // Stage02.DAL/Repositories/ProductRepository.cs
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsProductInOrderAsync(int productId)
        {
            return await _context.OrderItems
                .AnyAsync(oi => oi.ProductId == productId);
        }
    }

    // Stage02.DAL/Repositories/OrderRepository.cs
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }

### 5. DTOs (BLL)

    // Stage02.BLL/DTOs/LoginDto.cs
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Stage02.BLL/DTOs/RegisterDto.cs
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    // Stage02.BLL/DTOs/ProductDto.cs
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    // Stage02.BLL/DTOs/OrderDto.cs
    public class OrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

### 6. Service Interfaces (BLL)

    // Stage02.BLL/Services/IAuthService.cs
    public interface IAuthService
    {
        Task<User> LoginAsync(LoginDto dto);
        Task<User> RegisterAsync(RegisterDto dto);
    }

    // Stage02.BLL/Services/IProductService.cs
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(ProductDto dto);
        Task UpdateAsync(int id, ProductDto dto);
        Task DeleteAsync(int id);
    }

    // Stage02.BLL/Services/IOrderService.cs
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetMyOrdersAsync(int userId);
        Task<Order> CreateAsync(int userId, OrderDto dto);
    }

    // Stage02.BLL/Services/IUserService.cs
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task DeactivateAsync(int id);
    }

### 7. Service Implementations (BLL)

    // Stage02.BLL/Services/AuthService.cs
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> LoginAsync(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Username and password are required.");

            var user = await _userRepository.GetByUsernameAsync(dto.Username);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated.");

            if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            user.LastLogin = DateTime.Now;
            await _userRepository.UpdateAsync(user);

            return user;
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Password is required.");

            if (dto.Password != dto.ConfirmPassword)
                throw new ArgumentException("Passwords do not match.");

            if (dto.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters.");

            if (await _userRepository.UsernameExistsAsync(dto.Username))
                throw new InvalidOperationException("Username already exists.");

            var user = new User
            {
                Username = dto.Username.Trim(),
                PasswordHash = PasswordHasher.Hash(dto.Password),
                Role = "Customer",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            return await _userRepository.AddAsync(user);
        }
    }

    // Stage02.BLL/Services/ProductService.cs
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            return product;
        }

        public async Task<Product> AddAsync(ProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Product name is required.");

            if (dto.Price < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (dto.StockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            var product = new Product
            {
                Name = dto.Name.Trim(),
                Price = dto.Price,
                StockQuantity = dto.StockQuantity
            };

            return await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(int id, ProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Product name is required.");

            if (dto.Price < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (dto.StockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            product.Name = dto.Name.Trim();
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            var isInOrder = await _productRepository.IsProductInOrderAsync(id);
            if (isInOrder)
                throw new InvalidOperationException("Cannot delete product that is part of existing orders.");

            await _productRepository.DeleteAsync(id);
        }
    }

    // Stage02.BLL/Services/OrderService.cs
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<List<Order>> GetMyOrdersAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return await _orderRepository.GetByUserIdAsync(userId);
        }

        public async Task<Order> CreateAsync(int userId, OrderDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("Order must contain at least one item.");

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                Items = new List<OrderItem>()
            };

            foreach (var itemDto in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);

                if (product == null)
                    throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} not found.");

                if (itemDto.Quantity <= 0)
                    throw new ArgumentException($"Invalid quantity for product {product.Name}.");

                if (product.StockQuantity < itemDto.Quantity)
                    throw new InvalidOperationException(
                        $"Not enough stock for {product.Name}. Available: {product.StockQuantity}, Requested: {itemDto.Quantity}");

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                });

                product.StockQuantity -= itemDto.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            return await _orderRepository.AddAsync(order);
        }
    }

    // Stage02.BLL/Services/UserService.cs
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");
            return user;
        }

        public async Task DeactivateAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
        }
    }

### 8. Password Hasher (Common)

    // Stage02.Common/Helpers/PasswordHasher.cs
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool Verify(string password, string hash)
        {
            return Hash(password) == hash;
        }
    }

### 9. UI Layer with Role-Based Menu

    // Stage02.UI/Program.cs
    public class Program
    {
        private static IAuthService _authService;
        private static IProductService _productService;
        private static IOrderService _orderService;
        private static IUserService _userService;
        private static User _currentUser;

        public static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            _authService = serviceProvider.GetRequiredService<IAuthService>();
            _productService = serviceProvider.GetRequiredService<IProductService>();
            _orderService = serviceProvider.GetRequiredService<IOrderService>();
            _userService = serviceProvider.GetRequiredService<IUserService>();

            var context = serviceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
            await SeedData(context);

            await RunAsync();
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Server=.;Database=Stage02DB;Trusted_Connection=True;TrustServerCertificate=True"));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        private static async Task SeedData(AppDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                var admin = new User
                {
                    Username = "admin",
                    PasswordHash = PasswordHasher.Hash("admin123"),
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                var customer = new User
                {
                    Username = "customer",
                    PasswordHash = PasswordHasher.Hash("customer123"),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                await context.Users.AddRangeAsync(admin, customer);

                var products = new List<Product>
                {
                    new Product { Name = "Laptop", Price = 999.99m, StockQuantity = 10 },
                    new Product { Name = "Mouse", Price = 29.99m, StockQuantity = 50 },
                    new Product { Name = "Keyboard", Price = 79.99m, StockQuantity = 30 }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }

        private static async Task RunAsync()
        {
            while (true)
            {
                if (_currentUser == null)
                {
                    ShowAuthMenu();
                    var choice = Console.ReadLine();
                    await HandleAuthChoice(choice);
                }
                else
                {
                    ShowMainMenu();
                    var choice = Console.ReadLine();
                    await HandleMainChoice(choice);
                }
            }
        }

        private static void ShowAuthMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Order Management System ===");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.Write("Select: ");
        }

        private static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Order Management System ===");
            Console.WriteLine($"User: {_currentUser.Username} ({_currentUser.Role})");
            Console.WriteLine("-----------------------------------");

            if (_currentUser.Role == "Admin")
            {
                Console.WriteLine("1. Product Management");
                Console.WriteLine("2. View All Orders");
                Console.WriteLine("3. User Management");
                Console.WriteLine("4. Create Order");
                Console.WriteLine("5. My Orders");
                Console.WriteLine("6. Logout");
            }
            else
            {
                Console.WriteLine("1. Create Order");
                Console.WriteLine("2. My Orders");
                Console.WriteLine("3. Logout");
            }

            Console.Write("Select: ");
        }

        private static async Task HandleAuthChoice(string choice)
        {
            try
            {
                switch (choice)
                {
                    case "1":
                        await HandleLogin();
                        break;
                    case "2":
                        await HandleRegister();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        await Task.Delay(1000);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ {ex.Message}");
                await Task.Delay(2000);
            }
        }

        private static async Task HandleLogin()
        {
            Console.Clear();
            Console.WriteLine("=== Login ===");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = ReadPassword();

            var loginDto = new LoginDto { Username = username, Password = password };
            _currentUser = await _authService.LoginAsync(loginDto);
            Console.WriteLine($"✅ Welcome, {_currentUser.Username}!");
            await Task.Delay(1000);
        }

        private static async Task HandleRegister()
        {
            Console.Clear();
            Console.WriteLine("=== Register ===");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = ReadPassword();
            Console.Write("Confirm Password: ");
            var confirmPassword = ReadPassword();

            var registerDto = new RegisterDto
            {
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            await _authService.RegisterAsync(registerDto);
            Console.WriteLine("✅ Registration successful! Please login.");
            await Task.Delay(1000);
        }

        private static async Task HandleMainChoice(string choice)
        {
            try
            {
                if (_currentUser.Role == "Admin")
                {
                    await HandleAdminChoice(choice);
                }
                else
                {
                    await HandleCustomerChoice(choice);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ {ex.Message}");
                Console.ReadKey();
            }
        }

        private static async Task HandleAdminChoice(string choice)
        {
            switch (choice)
            {
                case "1": await ManageProducts(); break;
                case "2": await ViewAllOrders(); break;
                case "3": await ManageUsers(); break;
                case "4": await CreateOrder(); break;
                case "5": await ViewMyOrders(); break;
                case "6": _currentUser = null; break;
                default:
                    Console.WriteLine("Invalid option!");
                    await Task.Delay(1000);
                    break;
            }
        }

        private static async Task HandleCustomerChoice(string choice)
        {
            switch (choice)
            {
                case "1": await CreateOrder(); break;
                case "2": await ViewMyOrders(); break;
                case "3": _currentUser = null; break;
                default:
                    Console.WriteLine("Invalid option!");
                    await Task.Delay(1000);
                    break;
            }
        }

        private static string ReadPassword()
        {
            string pwd = "";
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && pwd.Length > 0)
                {
                    pwd = pwd[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    pwd += key.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return pwd;
        }

        private static async Task ManageProducts()
        {
            // Product CRUD operations using _productService
        }

        private static async Task ViewAllOrders()
        {
            var orders = await _orderService.GetAllAsync();
            // Display orders
        }

        private static async Task ViewMyOrders()
        {
            var orders = await _orderService.GetMyOrdersAsync(_currentUser.Id);
            // Display orders
        }

        private static async Task CreateOrder()
        {
            // Create order using _orderService
        }

        private static async Task ManageUsers()
        {
            // User management using _userService
        }
    }

## ✅ Architectural Benefits

| Benefit | Description |
|---------|-------------|
| **Separation of Concerns** | Each layer has a single, well-defined responsibility |
| **Testability** | Services can be unit tested with mock repositories |
| **Maintainability** | Changes in one layer don't cascade to others |
| **Database Persistence** | Data survives application restart with SQL Server |
| **Dependency Injection** | Loose coupling through interfaces |
| **Role-Based UI** | Menu adapts dynamically based on user role |
| **Input Validation** | Business rules enforced in service layer |
| **Repository Pattern** | Data access abstracted behind interfaces |

## ⚠️ Remaining Challenges

| Challenge | Description | Solved in Stage |
|-----------|-------------|----------------|
| Direct DAL dependency in BLL | BLL references EF Core via repositories | Stage 03 |
| Console UI limitation | Not suitable for web/mobile clients | Stage 04 |
| Session-based auth | Not scalable for distributed systems | Stage 05 |
| Validation in services | Mixed with business logic | Stage 08 |
| No transaction management | Complex operations lack atomicity | Stage 15 |

## 📊 Database Schema

    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY,
        Username NVARCHAR(50) UNIQUE NOT NULL,
        PasswordHash NVARCHAR(255) NOT NULL,
        Role NVARCHAR(20) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL,
        LastLogin DATETIME2 NULL
    );

    CREATE TABLE Products (
        Id INT PRIMARY KEY IDENTITY,
        Name NVARCHAR(100) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        StockQuantity INT NOT NULL
    );

    CREATE TABLE Orders (
        Id INT PRIMARY KEY IDENTITY,
        UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
        OrderDate DATETIME2 NOT NULL,
        Status INT NOT NULL
    );

    CREATE TABLE OrderItems (
        Id INT PRIMARY KEY IDENTITY,
        OrderId INT NOT NULL FOREIGN KEY REFERENCES Orders(Id),
        ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL
    );

## 🚀 Next Steps
In **Stage 03: Clean Architecture**, we will:
- Invert dependencies (apply DIP)
- Move EF Core to Infrastructure layer
- Make Domain layer completely independent
- Introduce Application layer with use cases

---

**End of Stage 02 | Proceed to Stage 03: Clean Architecture**


# Deep Dive: Principles & Patterns in Layered Architecture

## 📚 Questions & Answers from Stage 02

---

## ۱. Definition of Each Layer and Its Contents

### 🖥️ UI Layer (Presentation Layer)
**Responsibility:** Display information and receive user input only.

**What belongs here:**
- Console UI (menus, display logic)
- Web API Controllers (in future stages)
- Razor Pages / Blazor Components (in future stages)
- ViewModels (for display purposes)
- appsettings.json

**What does NOT belong here:**
- ❌ Business logic
- ❌ Direct database access
- ❌ Complex validation rules

---

### 🧠 BLL (Business Logic Layer)
**Responsibility:** Business logic, business rules, and validation orchestration.

**What belongs here:**
- Services (AuthService, ProductService, OrderService)
- DTOs (Data Transfer Objects)
- Validators (FluentValidation in later stages)
- Mapping profiles (Mapster/AutoMapper in later stages)
- Business rules enforcement

**What does NOT belong here:**
- ❌ Direct DbContext access
- ❌ UI-specific code
- ❌ Raw SQL queries

---

### 💾 DAL (Data Access Layer)
**Responsibility:** Store and retrieve data from the database.

**What belongs here:**
- DbContext (AppDbContext)
- Entity Classes (User, Product, Order, OrderItem)
- Repository Implementations
- Entity Configurations (Fluent API)
- Migrations

**What does NOT belong here:**
- ❌ Business logic
- ❌ UI code
- ❌ Business validation rules

---

### 🔧 Common (Shared Utilities)
**Responsibility:** Code shared across layers.

**What belongs here:**
- Helpers (PasswordHasher)
- Constants
- Extension methods
- Custom exceptions

---

## ۲. Why Use Repository Pattern and Interfaces?

### Problem: What Happens If We Use DbContext Directly in Services?

```csharp
// ❌ BAD - Direct dependency on EF Core
public class ProductService
{
    private readonly AppDbContext _context;
    
    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }
}
```

### Problems with This Approach:

| Problem | Explanation | Real-World Example |
|---------|-------------|-------------------|
| **1. Hard to Test** | Requires real database for every test | Cannot write unit tests without SQL Server |
| **2. Code Duplication** | Query logic repeated in every service | Include/Where/OrderBy duplicated everywhere |
| **3. Database Coupling** | Changing database requires changing all services | SQL Server to MongoDB = Rewrite entire BLL |
| **4. SRP Violation** | Service has two reasons to change | Business rules + Data access logic |

### Solution: Repository Pattern with Interface

```csharp
// ✅ GOOD - Depends on abstraction
public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
}

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    // Actual implementation
}

public class ProductService
{
    private readonly IProductRepository _repository;
    
    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }
}
```

---

## ۳. Why Use Interfaces for Repository?

### Benefits:

```
Testing:
    IProductRepository mockRepo = new MockProductRepository();
    var service = new ProductService(mockRepo);  // Test without real database

Database Flexibility:
    IProductRepository → SqlProductRepository
    IProductRepository → MongoProductRepository
    IProductRepository → ApiProductRepository

Decorator Pattern:
    IProductRepository → CachedProductRepository → SqlProductRepository
    IProductRepository → LoggedProductRepository → SqlProductRepository
```

### Key Insight:
The interface is a **contract**. It says "what" needs to be done, not "how". The implementation can change without affecting consumers.

---

## ۴. Why Use Interfaces for Services?

### Question: Why Not Inject the Service Class Directly?

```csharp
// ❌ BAD - Tight coupling
public class Program
{
    private static ProductService _productService;
    
    public Program(ProductService productService)
    {
        _productService = productService;
    }
}

// ✅ GOOD - Loose coupling
public class Program
{
    private static IProductService _productService;
    
    public Program(IProductService productService)
    {
        _productService = productService;
    }
}
```

### Reasons for Service Interfaces:

| Reason | Explanation | Example |
|--------|-------------|---------|
| **1. Testability** | Can mock the service easily | `Mock<IProductService>` |
| **2. Multiple Implementations** | Real and fake implementations | `ProductService` and `FakeProductService` |
| **3. Decorator Pattern** | Can wrap service with additional behavior | `CachedProductService` → `ProductService` |
| **4. Proxy Pattern** | Can add logging, caching, retry | `LoggingProductService` → `ProductService` |
| **5. DIP Compliance** | Depends on abstraction not concretion | Dependency Inversion Principle |

---

## ۵. Real-World Example: Why Interfaces Matter

### Scenario: Adding Caching Without Interfaces

```csharp
// ❌ BAD - Must modify original class
public class ProductService
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;  // New dependency added
    
    public async Task<List<Product>> GetAllAsync()
    {
        // Cache logic mixed with business logic
        // Original code gets polluted
    }
}
```

### Scenario: Adding Caching With Interfaces

```csharp
// ✅ GOOD - Original class unchanged
public class CachedProductService : IProductService
{
    private readonly IProductService _innerService;
    private readonly IMemoryCache _cache;
    
    public async Task<List<Product>> GetAllAsync()
    {
        return await _cache.GetOrCreateAsync("products", async entry =>
        {
            return await _innerService.GetAllAsync();  // Called only when needed
        });
    }
}

// DI Registration:
services.AddScoped<IProductService>(provider =>
{
    var realService = new ProductService(
        provider.GetRequiredService<IProductRepository>());
    var cache = provider.GetRequiredService<IMemoryCache>();
    return new CachedProductService(realService, cache);
});
```

---

## ۶. Where Does Validation Belong?

### Two Types of Validation:

```
Structural Validation:
    "Can this data even exist?"
    Examples: Username not null, Price not negative, Email format
    
    → Belongs in Entity (DAL Layer)

Business Validation:
    "Is this data valid in current context?"
    Examples: Username not duplicate, Product has enough stock
    
    → Belongs in Service (BLL Layer)
```

### Example:

```csharp
// ✅ Structural Validation → Entity (DAL)
public class User
{
    public string Username { get; private set; }
    
    public User(string usernameeries)
    {
        // Structural: Username must exist
        Username = string.IsNullOrWhiteSpace(username) 
            ? throw new ArgumentNullException(nameof(username)) 
            : username;
    }
}

// ✅ Business Validation → Service (BLL)
public class AuthService : IAuthService
{
    public async Task<User> RegisterAsync(RegisterDto dto)
    {
        // Business: Username must be unique
        if (await _userRepository.UsernameExistsAsync(dto.Username))
            throw new InvalidOperationException("Username already exists.");
        
        var user = new User(dto.Username);
        return await _userRepository.AddAsync(user);
    }
}
```

---

## ۷. Anemic vs Rich Domain Model

### Anemic Domain Model (Stage 02):
```
Entity is just a data holder.
All logic lives in Services.
"Entity = Glorified DTO"
```

```csharp
// Anemic - Entity has no behavior
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public bool IsActive { get; set; }
    // No methods, no logic
}

// All logic in Service
public class UserService
{
    public async Task DeactivateUser(int userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        user.IsActive = false;  // Logic outside entity
        await _repository.UpdateAsync(user);
    }
}
```

### Rich Domain Model (Stage 03):
```
Entity has behavior and business rules.
Service is just an orchestrator.
"Entity protects its own invariants"
```

```csharp
// Rich - Entity with behavior
public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; }
    public bool IsActive { get; private set; }
    
    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Already deactivated.");
        
        IsActive = false;
        // Can raise Domain Events here
    }
}

// Service as orchestrator
public class UserService
{
    public async Task DeactivateUser(int userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        user.Deactivate();  // Logic inside entity
        await _repository.UpdateAsync(user);
    }
}
```

### When to Use Which:

| Anemic Model | Rich Domain Model |
|--------------|-------------------|
| Simple CRUD apps | Complex business logic |
| Small projects | Enterprise applications |
| Stage 02 use case | Stage 03 use case |
| Easier to learn | More maintainable long-term |
| Logic scattered | Logic encapsulated |

---

## 🎯 Golden Rules Summary

```
1. Interface Rule:
   Any class with logic → Needs Interface
   Any class that might change → Needs Interface
   Any class you want to test → Needs Interface

2. Validation Rule:
   "Can this exist?" → Entity (Structural)
   "Is this allowed?" → Service (Business)

3. Layer Rule:
   UI → Presentation only
   BLL → Business logic only
   DAL → Data access only
   Common → Shared utilities

4. Exception:
   DTOs → Don't need interfaces
   Entities → May not need interfaces (but can)
   Simple helpers → Don't need interfaces
```

---

## 📊 Stage 02 Decision Log

| Decision | Why We Chose It | Trade-off |
|----------|-----------------|-----------|
| Anemic Model | Simpler to understand first | Less encapsulation |
| Repository Pattern | Testability + Database flexibility | More code to write |
| Interface for Services | Testability + Decorator pattern | Extra abstraction |
| Validation in Services | Quicker to implement | Logic not in Entity |
| Console UI | No web dependency yet | Limited client support |

---

## 🚀 What Changes in Stage 03

In **Stage 03 (Clean Architecture)**, we will:
- Move to Rich Domain Model
- Entities will have behavior and enforce invariants
- Domain layer will be completely independent
- Validation moves partially into Entities
- Repository interfaces move to Domain layer
- EF Core becomes an infrastructure detail

---

**End of Deep Dive | Keep Learning!**