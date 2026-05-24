namespace Step022.Application.Products;

public class ProductService : IProductService 
{

    private readonly IProductRepository _productRepo;

    public ProductService(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }


    // ============================================================
    // Create Product
    // ============================================================
    public Product CreateProduct(string name, decimal price, int stockQuantity)
    {
        // ⭐ Entity خودش اعتبارسنجی می‌کنه
        var product = Product.Create(name, price, stockQuantity);

        // ⭐ Application فقط ذخیره می‌کنه
        _productRepo.Add(product);

        return product;
    }


    // ============================================================
    // Get Products
    // ============================================================
    public Product GetProduct(int id)
    {
        var product = _productRepo.GetById(id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        return product;
    }

    public List<Product> GetAllProducts()
    {
        return _productRepo.GetAll();
    }


    // ============================================================
    // Update Product
    // ============================================================
    public Product UpdateProduct(int id, string name, decimal price)
    {
        var product = GetProduct(id);

        // ⭐ Entity خودش اعتبارسنجی و آپدیت می‌کنه
        product.UpdateInfo(name, price);

        _productRepo.Update(product);

        return product;
    }


    // ============================================================
    // Delete Product
    // ============================================================
    public void DeleteProduct(int id)
    {
        var product = GetProduct(id);
        _productRepo.Delete(id);
    }


    // ============================================================
    // Reduce Stock
    // ============================================================
    public void ReduceStock(int productId, int quantity)
    {
        var product = GetProduct(productId);

        // ⭐ Entity خودش منطق کم کردن موجودی رو داره
        product.ReduceStock(quantity);

        _productRepo.Update(product);
    }

}
