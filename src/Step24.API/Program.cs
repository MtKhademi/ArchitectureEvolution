using Step017.Domain.Repositories;
using Step022.Application.Orders;
using Step022.Application.Products;
using Step022.Application.Users;
using Step03.Infrastructure.Repositories;
using Step21.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddSingleton<DatabaseContext>();

builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<IUserService, UserService>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

using (var scoped = app.Services.CreateScope())
{
    var productRepo = scoped.ServiceProvider.GetService<IProductService>();
    SeedData(productRepo);
}

app.Run();



static void SeedData(IProductService productService)
{
    if (productService.GetAllProducts().Any()) return;

    Console.WriteLine("Create seed data");
    productService.CreateProduct("Red Apple", 1.50m, 100);
    productService.CreateProduct("Banana Bunch", 0.80m, 150);
    productService.CreateProduct("Fresh Orange", 1.20m, 80);
    productService.CreateProduct("Green Grapes", 3.50m, 50);
    productService.CreateProduct("Strawberry Pack", 4.00m, 30);
    productService.CreateProduct("Whole Milk", 2.50m, 60);
    productService.CreateProduct("Bread Loaf", 1.80m, 40);
    productService.CreateProduct("Chicken Breast", 7.99m, 25);
    productService.CreateProduct("Pasta Pack", 1.50m, 70);
    productService.CreateProduct("Tomato Sauce", 2.20m, 45);
}