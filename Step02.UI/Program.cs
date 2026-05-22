using Step02.BLL.Repositorties;
using Step02.BLL.Services;
using Step02.DAL.Data;
using Step02.DAL.Repositories;
using Step02.UI;

var dbContext = new DatabaseContext();
IProductRepository productLoggingRepository = new ProductLoggingRepository(dbContext);
IProductService _productService = new ProductLoggingService(
    new ProductService(productLoggingRepository));

ProductUI.SeedData(_productService);

while (true)
{
    var choose = ShowMenu();
    switch (choose)
    {
        case "1":
            ProductUI.AddProduct(_productService);
            break;
        case "2":
            ProductUI.ViewAllProducts(_productService);
            break;
        case "3":
            ProductUI.UpdateAProduct(_productService);
            break;
        case "4":
            ProductUI.DeleteAProduct(_productService);
            break;
        //case "5":
        //    CreateANewOrder();
        //    break;
        //case "6":
        //    ViewAllOrders();
        //    break;
        case "7":
            Console.WriteLine("Exiting the program...");
            break;
        default:
            Console.WriteLine("Invalid option. Please choose a number between 1 and 5.");
            break;
    }

    if (choose == "7") break;
}

string ShowMenu()
{
    Console.WriteLine("========== Product management system ==========");
    Console.WriteLine(" 1. Add a new product");
    Console.WriteLine(" 2. View all products");
    Console.WriteLine(" 3. Update a product");
    Console.WriteLine(" 4. Delete a product");
    Console.WriteLine(" 5. Create a new order");
    Console.WriteLine(" 6. View all orders");
    Console.WriteLine(" 7. Exit");

    Console.Write("Please choose an option (1-5): ");
    return Console.ReadLine();
}
