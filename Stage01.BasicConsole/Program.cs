Console.WriteLine("Step01 -> Basic console");
//using Stage01.BasicConsole;

//List<Product> products = new List<Product>();

//List<Order> orders = new List<Order>();
//SeedData();

//while (true)
//{
//    var choose = ShowMenu();
//    switch (choose)
//    {
//        case "1":
//            AddProduct();
//            break;
//        case "2":
//            ViewAllProducts();
//            break;
//        case "3":
//            UpdateAProduct();
//            break;
//        case "4":
//            DeleteAProduct();
//            break;
//        case "5":
//            CreateANewOrder();
//            break;
//        case "6":
//            ViewAllOrders();
//            break;
//        case "7":


//while (true)
//{
//    Console.WriteLine("========== Product management system ==========");
//    Console.WriteLine(" 1. Create a new product");
//    Console.WriteLine(" 2. View all products");
//    Console.WriteLine(" 3. Update a product");
//    Console.WriteLine(" 4. Delete a product");
//    Console.WriteLine(" 5. Exit");

//    Console.Write("Please choose an option (1-5): ");
//    var choose = Console.ReadLine();
//    switch (choose)
//    {
//        case "1":
//            Console.WriteLine("Creating a new product...");
//            Console.Write("Please enter product name : ");
//            var productName = Console.ReadLine();
//            Console.Write("Please enter product price : ");
//            var productPrice = Console.ReadLine();
//            Console.Write("Please enter product stock quantity : ");
//            var productStockQuantity = Console.ReadLine();

//            var product = new Product
//            {
//                Id = products.Any() ? products.OrderByDescending(x => x.Id).First().Id + 1 : 1,
//                Name = productName,
//                Price = decimal.Parse(productPrice),
//                StockQuantity = int.Parse(productStockQuantity)
//            };
//            products.Add(product);
//            Console.WriteLine($"Product created successfully! : product id : {product.Id}");
//            break;
//        case "2":
//            Console.WriteLine("Viewing all products...");
//            break;
//        case "3":
//            Console.WriteLine("Updating a product...");
//            break;
//        case "4":
//            Console.WriteLine("Deleting a product...");
//            break;
//        case "5":
//            Console.WriteLine("Exiting the program...");
//            break;
//        default:
//            Console.WriteLine("Invalid option. Please choose a number between 1 and 5.");
//            break;
//    }


//    if (choose == "7") break;

//}


//string ShowMenu()
//{
//    Console.WriteLine("========== Product management system ==========");
//    Console.WriteLine(" 1. Add a new product");
//    Console.WriteLine(" 2. View all products");
//    Console.WriteLine(" 3. Update a product");
//    Console.WriteLine(" 4. Delete a product");
//    Console.WriteLine(" 5. Create a new order");
//    Console.WriteLine(" 6. View all orders");
//    Console.WriteLine(" 7. Exit");

//    Console.Write("Please choose an option (1-5): ");
//    return Console.ReadLine();
//}
//void SeedData()
//{
//    if (products.Any()) return;

//    products.Add(new Product { Id = 1, Name = "Red Apple", Price = 1.50m, StockQuantity = 100 });
//    products.Add(new Product { Id = 2, Name = "Banana Bunch", Price = 0.80m, StockQuantity = 150 });
//    products.Add(new Product { Id = 3, Name = "Fresh Orange", Price = 1.20m, StockQuantity = 80 });
//    products.Add(new Product { Id = 4, Name = "Green Grapes", Price = 3.50m, StockQuantity = 50 });
//    products.Add(new Product { Id = 5, Name = "Strawberry Pack", Price = 4.00m, StockQuantity = 30 });

//}

//void AddProduct()
//{
//    Console.Clear();
//    Console.WriteLine("======== Add new a product ============");
//    Console.Write("Please enter product name : ");

//    var productName = Console.ReadLine();
//    if (string.IsNullOrWhiteSpace(productName))
//    {
//        Console.WriteLine($"Invalid product name . ");
//        return;
//    }

//    Console.Write("Please enter product price : ");
//    if (!decimal.TryParse(Console.ReadLine(), out var productPrice))
//    {
//        Console.WriteLine("Invalid product price");
//        return;
//    }

//    Console.Write("Please enter product stock quantity : ");
//    if (!int.TryParse(Console.ReadLine(), out var productStockQuantity))
//    {
//        Console.WriteLine("Invalid product stock quantity .");
//        return;
//    }
//    var product = new Product
//    {
//        Id = products.Any() ? products.OrderByDescending(x => x.Id).First().Id + 1 : 1,
//        Name = productName,
//        Price = productPrice,
//        StockQuantity = productStockQuantity
//    };
//    products.Add(product);
//    Console.Clear();
//    Console.WriteLine($"Product created successfully! : product id : {product.Id}");
//}

//void ViewAllProducts()
//{
//    Console.Clear();
//    Console.WriteLine("======== View all products ============");
//    foreach (var p in products)
//    {
//        Console.WriteLine(p);
//    }
//}

//void UpdateAProduct()
//{
//    Console.Clear();
//    Console.WriteLine("======== Update a product ============");

//    ViewAllProducts();

//    Console.WriteLine("==============================");
//    Console.Write("Please enter product id : ");
//    if (!int.TryParse(Console.ReadLine(), out var productId))
//    {
//        Console.WriteLine("Invalid id .");
//        return;
//    }
//    var productToUpdate = products.FirstOrDefault(product => product.Id == productId);
//    if (productToUpdate is null)
//    {
//        Console.WriteLine("not found product");
//        return;
//    }
//    Console.Write($"Please enter new name - ({productToUpdate.Name}) : ");
//    var newName = Console.ReadLine();
//    if (string.IsNullOrEmpty(newName))
//    {
//        Console.WriteLine("invalid product name");
//        return;
//    }
//    productToUpdate.Name = newName;
//    Console.Write($"Please enter new price - ({productToUpdate.Price}) : ");
//    productToUpdate.Price = decimal.Parse(Console.ReadLine());
//    Console.Write($"Please enter new quantity - ({productToUpdate.StockQuantity}) : ");
//    productToUpdate.StockQuantity = int.Parse(Console.ReadLine());

//    Console.Clear();
//    Console.WriteLine("Successfully update product");
//    Console.WriteLine(productToUpdate);

//}

//void DeleteAProduct()
//{
//    Console.Clear();
//    Console.WriteLine("======== Delete a product ============");

//    ViewAllProducts();

//    Console.WriteLine("==============================");
//    Console.Write("Please enter product id : ");
//    var productId2 = int.Parse(Console.ReadLine());
//    var productToDelete = products.FirstOrDefault(product => product.Id == productId2);
//    products.Remove(productToDelete);

//    Console.Clear();
//    Console.WriteLine("Delete product successfully . ");
//    Console.WriteLine($"Deleted product : {productToDelete}");

//}

//void CreateANewOrder()
//{
//    Console.Clear();
//    Console.WriteLine("========= Create a new order ==========");

//    ViewAllProducts();

//    var order = new Order
//    {
//        Id = 1,
//    };

//    while (true)
//    {
//        Console.Write($"Please enter a product id (0 to finish) : ");
//        int productId = int.Parse(Console.ReadLine());

//        if (productId == 0) break;

//        var Prodcut = products.FirstOrDefault(product => product.Id == productId);

//        Console.Write($"Please enter count [{Prodcut.StockQuantity}] : ");
//        var count = int.Parse(Console.ReadLine());

//        order.Items.Add(new OrderItem
//        {
//            ProductId = productId,
//            Quantity = count,
//            UnitPrice = Prodcut.Price
//        });
//    }

//    Console.Clear();
//    Console.WriteLine("Order create successfully . ");
//    Console.WriteLine(order);

//    orders.Add(order);
//}



//void ViewAllOrders()
//{
//    Console.Clear();
//    Console.WriteLine("=========== View all orders ==========");
//    foreach (var item in orders)
//    {
//        Console.WriteLine(item);
//    }
//}