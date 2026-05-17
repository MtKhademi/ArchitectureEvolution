using Stage01.BasicConsole;

List<Product> products = new List<Product>();

while (true)
{
    Console.WriteLine("========== Product management system ==========");
    Console.WriteLine(" 1. Create a new product");
    Console.WriteLine(" 2. View all products");
    Console.WriteLine(" 3. Update a product");
    Console.WriteLine(" 4. Delete a product");
    Console.WriteLine(" 5. Exit");

    Console.Write("Please choose an option (1-5): ");
    var choose = Console.ReadLine();
    switch (choose)
    {
        case "1":
            Console.WriteLine("Creating a new product...");
            Console.Write("Please enter product name : ");
            var productName = Console.ReadLine();
            Console.Write("Please enter product price : ");
            var productPrice = Console.ReadLine();
            Console.Write("Please enter product stock quantity : ");
            var productStockQuantity = Console.ReadLine();

            var product = new Product
            {
                Id = products.Any() ? products.OrderByDescending(x => x.Id).First().Id + 1 : 1,
                Name = productName,
                Price = decimal.Parse(productPrice),
                StockQuantity = int.Parse(productStockQuantity)
            };
            products.Add(product);
            Console.WriteLine($"Product created successfully! : product id : {product.Id}");
            break;
        case "2":
            Console.WriteLine("Viewing all products...");
            break;
        case "3":
            Console.WriteLine("Updating a product...");
            break;
        case "4":
            Console.WriteLine("Deleting a product...");
            break;
        case "5":
            Console.WriteLine("Exiting the program...");
            break;
        default:
            Console.WriteLine("Invalid option. Please choose a number between 1 and 5.");
            break;
    }

    if (choose == "5") break;

}