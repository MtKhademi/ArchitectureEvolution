//using Step017.Domain.Enums;

//namespace Step02.UI;

//public static class OrderUI
//{
//    private static IOrderService _orderService;
//    private static IProductService _productService;

//    public static int _currentUserId;
//    public static string _currentUserRole;

//    public static void Initialize(IOrderService orderService, IProductService productService)
//    {
//        _orderService = orderService;
//        _productService = productService;
//    }

//    // ============================================================
//    // منوی اصلی - بر اساس Role تغییر می‌کنه
//    // ============================================================
//    public static void ShowOrderMenu()
//    {
//        while (true)
//        {
//            Console.Clear();
//            Console.WriteLine("╔══════════════════════════════════════╗");
//            Console.WriteLine("║         ORDER MANAGEMENT            ║");
//            Console.WriteLine("╠══════════════════════════════════════╣");
//            Console.WriteLine("║  1. Create New Order                ║");

//            if (_currentUserRole == "Admin")
//            {
//                Console.WriteLine("║  2. View All Orders                 ║");
//                Console.WriteLine("║  3. View Orders by Status           ║");
//                Console.WriteLine("║  4. View Order Details              ║");
//                Console.WriteLine("║  5. Update Order Status             ║");
//                Console.WriteLine("║  6. Cancel Any Order                ║");
//                Console.WriteLine("║  7. Back to Main Menu               ║");
//            }
//            else
//            {
//                Console.WriteLine("║  2. View My Orders                  ║");
//                Console.WriteLine("║  3. View My Order Details           ║");
//                Console.WriteLine("║  4. Cancel My Order                 ║");
//                Console.WriteLine("║  5. Back to Main Menu               ║");
//            }

//            Console.WriteLine("╚══════════════════════════════════════╝");
//            Console.Write("   Select: ");

//            var choice = Console.ReadLine();

//            try
//            {
//                if (_currentUserRole == "Admin")
//                {
//                    switch (choice)
//                    {
//                        case "1": CreateNewOrder(); break;
//                        case "2": ViewAllOrders(); break;
//                        case "3": ViewOrdersByStatus(); break;
//                        case "4": ViewOrderDetails(); break;
//                        case "5": UpdateOrderStatus(); break;
//                        case "6": CancelOrder(); break;
//                        case "7": return;
//                        default: InvalidOption(); break;
//                    }
//                }
//                else
//                {
//                    switch (choice)
//                    {
//                        case "1": CreateNewOrder(); break;
//                        case "2": ViewMyOrders(); break;
//                        case "3": ViewMyOrderDetails(); break;
//                        case "4": CancelMyOrder(); break;
//                        case "5": return;
//                        default: InvalidOption(); break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ex.LogOnConsole();
//                Console.ReadKey();
//            }
//        }
//    }

//    // ============================================================
//    // ۱. ایجاد سفارش جدید
//    // ============================================================
//    private static void CreateNewOrder()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║         CREATE NEW ORDER             ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        // نمایش محصولات موجود
//        var products = _productService.GetAllProducts();

//        if (products.Count == 0)
//        {
//            Console.WriteLine("\n❌ No products available. Please add products first.");
//            Console.ReadKey();
//            return;
//        }

//        Console.WriteLine("\nAvailable Products:");
//        Console.WriteLine(new string('-', 60));
//        Console.WriteLine($"{"ID",-5} {"Name",-20} {"Price",-15} {"Stock",-10}");
//        Console.WriteLine(new string('-', 60));

//        foreach (var p in products)
//        {
//            Console.WriteLine($"{p.Id,-5} {p.Name,-20} {p.Price,13:C} {p.StockQuantity,8}");
//        }
//        Console.WriteLine(new string('-', 60));

//        // انتخاب محصولات
//        var selectedProducts = new List<int>();
//        var selectedQuantities = new List<int>();

//        Console.WriteLine("\nEnter product IDs and quantities (enter 0 to finish):");

//        while (true)
//        {
//            Console.Write("Product ID (0 = finish): ");
//            if (!int.TryParse(Console.ReadLine(), out var productId))
//            {
//                Console.WriteLine("Invalid input.");
//                continue;
//            }

//            if (productId == 0) break;

//            var product = _productService.GetProduct(productId);
           
//            if (selectedProducts.Contains(productId))
//            {
//                Console.WriteLine($"⚠️  Product '{product.Name}' already selected. Use update order to modify.");
//                continue;
//            }

//            Console.Write($"Quantity of '{product.Name}' (max {product.StockQuantity}): ");
//            if (!int.TryParse(Console.ReadLine(), out var quantity))
//            {
//                Console.WriteLine("Invalid quantity.");
//                continue;
//            }

//            if (quantity <= 0)
//            {
//                Console.WriteLine("Quantity must be positive.");
//                continue;
//            }

//            if (quantity > product.StockQuantity)
//            {
//                Console.WriteLine($"❌ Not enough stock! Available: {product.StockQuantity}");
//                continue;
//            }

//            selectedProducts.Add(productId);
//            selectedQuantities.Add(quantity);

//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine($"✅ {product.Name} x{quantity} added to cart.");
//            Console.ResetColor();
//        }

//        if (selectedProducts.Count == 0)
//        {
//            Console.WriteLine("\nOrder cancelled - no products selected.");
//            Console.ReadKey();
//            return;
//        }

//        // نمایش خلاصه سفارش قبل از ثبت نهایی
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║          ORDER SUMMARY               ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");
//        Console.WriteLine(new string('-', 50));

//        decimal totalAmount = 0;
//        for (int i = 0; i < selectedProducts.Count; i++)
//        {
//            var product = _productService.GetProduct(selectedProducts[i]);
//            var quantity = selectedQuantities[i];
//            var subtotal = product.Price * quantity;
//            totalAmount += subtotal;

//            Console.WriteLine($"  {product.Name,-20} x{quantity,-3} = {subtotal,10:C}");
//        }
//        Console.WriteLine(new string('-', 50));
//        Console.WriteLine($"  {"Total:",-26} {totalAmount,10:C}");
//        Console.WriteLine(new string('-', 50));

//        Console.Write("\nConfirm order? (y/n): ");
//        var confirm = Console.ReadLine()?.ToLower();

//        if (confirm != "y")
//        {
//            Console.WriteLine("Order cancelled by user.");
//            Console.ReadKey();
//            return;
//        }

//        // ثبت سفارش
//        var order = _orderService.CreateOrder(_currentUserId, selectedProducts, selectedQuantities);

//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine($"\n✅ Order #{order.Id} created successfully!");
//        Console.WriteLine($"   Status: {order.Status}");
//        Console.WriteLine($"   Items: {order.Items.Count}");
//        Console.WriteLine($"   Total: {order.TotalPrice:C}");
//        Console.ResetColor();

//        Console.ReadKey();
//    }

//    // ============================================================
//    // ۲. نمایش همه سفارش‌ها
//    // ============================================================
//    private static void ViewAllOrders()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║           ALL ORDERS                 ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        var orders = _orderService.GetAllOrders();

//        if (orders.Count == 0)
//        {
//            Console.WriteLine("\nNo orders found.");
//            Console.ReadKey();
//            return;
//        }

//        PrintOrdersTable(orders);

//        Console.ReadKey();
//    }

//    // ============================================================
//    // ۳. نمایش سفارش‌ها بر اساس وضعیت
//    // ============================================================
//    private static void ViewOrdersByStatus()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║        VIEW ORDERS BY STATUS         ║");
//        Console.WriteLine("╠══════════════════════════════════════╣");
//        Console.WriteLine("║  1. Pending                          ║");
//        Console.WriteLine("║  2. Confirmed                        ║");
//        Console.WriteLine("║  3. Shipped                          ║");
//        Console.WriteLine("║  4. Delivered                        ║");
//        Console.WriteLine("║  5. Cancelled                        ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");
//        Console.Write("Select status: ");

//        if (!int.TryParse(Console.ReadLine(), out var choice) || choice < 1 || choice > 5)
//        {
//            Console.WriteLine("Invalid choice.");
//            Console.ReadKey();
//            return;
//        }

//        var status = choice switch
//        {
//            1 => OrderStatus.Pending,
//            2 => OrderStatus.Confirmed,
//            3 => OrderStatus.Shipped,
//            4 => OrderStatus.Delivered,
//            5 => OrderStatus.Cancelled,
//            _ => throw new ArgumentOutOfRangeException()
//        };

//        var orders = _orderService.GetOrdersByStatus(status);

//        Console.Clear();
//        Console.WriteLine($"╔══════════════════════════════════════╗");
//        Console.WriteLine($"║     ORDERS - {status.ToString().ToUpper(),-16}    ║");
//        Console.WriteLine($"╚══════════════════════════════════════╝");

//        if (orders.Count == 0)
//        {
//            Console.WriteLine($"\nNo orders with status '{status}'.");
//            Console.ReadKey();
//            return;
//        }

//        PrintOrdersTable(orders);

//        Console.ReadKey();
//    }

//    // ============================================================
//    // ۴. نمایش جزئیات یک سفارش
//    // ============================================================
//    private static void ViewOrderDetails()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║        VIEW ORDER DETAILS            ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        Console.Write("Order ID: ");
//        if (!int.TryParse(Console.ReadLine(), out var orderId))
//        {
//            Console.WriteLine("Invalid ID.");
//            Console.ReadKey();
//            return;
//        }

//        var order = _orderService.GetOrderById(orderId);

//        PrintOrderDetail(order);

//        Console.ReadKey();
//    }

//    // ============================================================
//    // ۵. تغییر وضعیت سفارش
//    // ============================================================
//    private static void UpdateOrderStatus()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║       UPDATE ORDER STATUS            ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        Console.Write("Order ID: ");
//        if (!int.TryParse(Console.ReadLine(), out var orderId))
//        {
//            Console.WriteLine("Invalid ID.");
//            Console.ReadKey();
//            return;
//        }

//        var order = _orderService.GetOrderById(orderId);

//        PrintOrderDetail(order);

//        // نمایش وضعیت‌های مجاز
//        var currentStatus = order.Status;

//        if (currentStatus == OrderStatus.Delivered || currentStatus == OrderStatus.Cancelled)
//        {
//            Console.WriteLine($"\n❌ Order is already '{currentStatus}'. No further changes allowed.");
//            Console.ReadKey();
//            return;
//        }

//        Console.WriteLine("\nAllowed status transitions:");

//        var transitions = currentStatus switch
//        {
//            OrderStatus.Pending => new[] { OrderStatus.Confirmed, OrderStatus.Cancelled },
//            OrderStatus.Confirmed => new[] { OrderStatus.Shipped, OrderStatus.Cancelled },
//            OrderStatus.Shipped => new[] { OrderStatus.Delivered },
//            _ => Array.Empty<OrderStatus>()
//        };

//        for (int i = 0; i < transitions.Length; i++)
//        {
//            Console.WriteLine($"  {i + 1}. {transitions[i]}");
//        }

//        Console.Write("\nSelect new status: ");
//        if (!int.TryParse(Console.ReadLine(), out var choice) || choice < 1 || choice > transitions.Length)
//        {
//            Console.WriteLine("Invalid choice.");
//            Console.ReadKey();
//            return;
//        }

//        var newStatus = transitions[choice - 1];

//        // تایید نهایی
//        Console.Write($"Confirm change status from '{currentStatus}' to '{newStatus}'? (y/n): ");
//        var confirm = Console.ReadLine()?.ToLower();

//        if (confirm != "y")
//        {
//            Console.WriteLine("Status update cancelled.");
//            Console.ReadKey();
//            return;
//        }

//        _orderService.UpdateOrderStatus(orderId, newStatus);

//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine($"\n✅ Order #{orderId} status updated: {currentStatus} → {newStatus}");
//        Console.ResetColor();

//        Console.ReadKey();
//    }

//    // ============================================================
//    // ۶. کنسل کردن سفارش
//    // ============================================================
//    private static void CancelOrder()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║          CANCEL ORDER                ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        Console.Write("Order ID: ");
//        if (!int.TryParse(Console.ReadLine(), out var orderId))
//        {
//            Console.WriteLine("Invalid ID.");
//            Console.ReadKey();
//            return;
//        }

//        var order = _orderService.GetOrderById(orderId);

//        PrintOrderDetail(order);

//        if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Confirmed)
//        {
//            Console.WriteLine($"\n❌ Cannot cancel order with status '{order.Status}'.");
//            Console.WriteLine("   Only Pending or Confirmed orders can be cancelled.");
//            Console.ReadKey();
//            return;
//        }

//        Console.Write($"\n⚠️  Are you sure you want to cancel order #{orderId}? (y/n): ");
//        var confirm = Console.ReadLine()?.ToLower();

//        if (confirm != "y")
//        {
//            Console.WriteLine("Cancellation aborted.");
//            Console.ReadKey();
//            return;
//        }

//        _orderService.CancelOrder(orderId);

//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine($"\n✅ Order #{orderId} cancelled successfully.");
//        Console.WriteLine("   All items returned to stock.");
//        Console.ResetColor();

//        Console.ReadKey();
//    }

//    // ============================================================
//    // Helper: چاپ جدول سفارش‌ها
//    // ============================================================
//    private static void PrintOrdersTable(List<Order> orders)
//    {
//        Console.WriteLine(new string('-', 75));
//        Console.WriteLine($"{"ID",-5} {"Date",-20} {"Items",-8} {"Total",-15} {"Status",-15}");
//        Console.WriteLine(new string('-', 75));

//        foreach (var order in orders)
//        {
//            var statusColor = order.Status switch
//            {
//                OrderStatus.Pending => ConsoleColor.Yellow,
//                OrderStatus.Confirmed => ConsoleColor.Cyan,
//                OrderStatus.Shipped => ConsoleColor.Blue,
//                OrderStatus.Delivered => ConsoleColor.Green,
//                OrderStatus.Cancelled => ConsoleColor.Red,
//                _ => ConsoleColor.White
//            };

//            Console.Write($"{order.Id,-5} {order.OrderDate,-20:yyyy-MM-dd HH:mm} " +
//                          $"{order.Items.Count,-8} {order.TotalPrice,13:C}  ");

//            Console.ForegroundColor = statusColor;
//            Console.Write($"{order.Status,-15}");
//            Console.ResetColor();
//            Console.WriteLine();
//        }

//        Console.WriteLine(new string('-', 75));

//        // آمار کلی
//        var totalOrders = orders.Count;
//        var totalAmount = orders.Sum(o => o.TotalPrice);
//        var totalItems = orders.Sum(o => o.Items.Sum(i => i.Quantity));

//        Console.WriteLine($"  Total: {totalOrders} orders | {totalItems} items | {totalAmount:C}");
//    }

//    // ============================================================
//    // Helper: چاپ جزئیات یک سفارش
//    // ============================================================
//    private static void PrintOrderDetail(Order order)
//    {
//        Console.WriteLine("\n╔══════════════════════════════════════╗");
//        Console.WriteLine($" ║        ORDER #{order.Id,-8}          ║");
//        Console.WriteLine("  ╚══════════════════════════════════════╝");

//        // تعیین رنگ وضعیت
//        var statusColor = order.Status switch
//        {
//            OrderStatus.Pending => ConsoleColor.Yellow,
//            OrderStatus.Confirmed => ConsoleColor.Cyan,
//            OrderStatus.Shipped => ConsoleColor.Blue,
//            OrderStatus.Delivered => ConsoleColor.Green,
//            OrderStatus.Cancelled => ConsoleColor.Red,
//            _ => ConsoleColor.White
//        };

//        Console.WriteLine($"  Date:   {order.OrderDate:yyyy-MM-dd HH:mm:ss}");
//        Console.Write("  Status: ");
//        Console.ForegroundColor = statusColor;
//        Console.Write(order.Status);
//        Console.ResetColor();
//        Console.WriteLine();

//        Console.WriteLine("\n  Items:");
//        Console.WriteLine(new string('-', 50));
//        Console.WriteLine($"  {"Product",-20} {"Qty",-8} {"UnitPrice",-12} {"SubTotal",-10}");
//        Console.WriteLine(new string('-', 50));

//        foreach (var item in order.Items)
//        {
//            Console.WriteLine($"  {item.ProductId,-20} {item.Quantity,-8} {item.UnitPrice,10:C}  {item.TotalPrice,10:C}");
//        }

//        Console.WriteLine(new string('-', 50));
//        Console.WriteLine($"  {"Total:",-30} {order.TotalPrice,10:C}");
//        Console.WriteLine(new string('-', 50));
//    }


//    // ============================================================
//    // ViewMyOrders - فقط سفارش‌های کاربر جاری
//    // ============================================================
//    private static void ViewMyOrders()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║            MY ORDERS                ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        var orders = _orderService.GetMyOrders(_currentUserId);

//        if (orders.Count == 0)
//        {
//            Console.WriteLine("\nYou have no orders yet.");
//            Console.ReadKey();
//            return;
//        }

//        PrintOrdersTable(orders);
//        Console.ReadKey();
//    }

//    // ============================================================
//    // CancelMyOrder - فقط سفارش خودش
//    // ============================================================
//    private static void CancelMyOrder()
//    {
//        Console.Clear();
//        Console.Write("Order ID to cancel: ");
//        if (!int.TryParse(Console.ReadLine(), out var orderId)) return;

//        _orderService.CancelMyOrder(orderId, _currentUserId);

//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine($"✅ Order #{orderId} cancelled. Items returned to stock.");
//        Console.ResetColor();
//        Console.ReadKey();
//    }

//    // ============================================================
//    // ViewMyOrderDetails - فقط سفارش خودش رو ببینه
//    // ============================================================
//    private static void ViewMyOrderDetails()
//    {
//        Console.Clear();
//        Console.WriteLine("╔══════════════════════════════════════╗");
//        Console.WriteLine("║        MY ORDER DETAILS              ║");
//        Console.WriteLine("╚══════════════════════════════════════╝");

//        // اول سفارش‌های خودش رو نشون می‌دیم که ID رو ببینه
//        var myOrders = _orderService.GetMyOrders(_currentUserId);

//        if (myOrders.Count == 0)
//        {
//            Console.WriteLine("\nYou have no orders.");
//            Console.ReadKey();
//            return;
//        }

//        PrintOrdersTable(myOrders);

//        Console.Write("\nOrder ID to view details: ");
//        if (!int.TryParse(Console.ReadLine(), out var orderId))
//        {
//            Console.WriteLine("❌ Invalid ID.");
//            Console.ReadKey();
//            return;
//        }

//        // چک مالکیت
//        var order = _orderService.GetOrderById(orderId);

//        if (order.UserId != _currentUserId)
//        {
//            Console.ForegroundColor = ConsoleColor.Red;
//            Console.WriteLine("❌ This is not your order!");
//            Console.ResetColor();
//            Console.ReadKey();
//            return;
//        }

//        PrintOrderDetail(order);
//        Console.ReadKey();
//    }

//    // ============================================================
//    // InvalidOption - پیام مشترک برای گزینه نامعتبر
//    // ============================================================
//    private static void InvalidOption()
//    {
//        Console.ForegroundColor = ConsoleColor.Yellow;
//        Console.WriteLine("⚠️  Invalid option! Please try again.");
//        Console.ResetColor();
//        Thread.Sleep(1000);
//    }
//}