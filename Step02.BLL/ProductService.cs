using Step02.DAL.Data;
using Step02.DAL.Entities;

namespace Step02.BLL;

public class ProductService
{
    private readonly DatabaseContext _dbContext;

    public ProductService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Product AddNewProduct(string name, decimal price, int quantity)
    {
        //- Validation
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
        if (price <= 0) throw new ArgumentOutOfRangeException("Price can not be or less zero.");
        if (quantity < 0) throw new ArgumentOutOfRangeException("Quantity can not be negative.");

        //- add to db
        var product = new Product
        {
            Id = DatabaseContext.ProductNextId++,
            Name = name,
            Price = price,
            StockQuantity = quantity
        };

        _dbContext.Products.Add(product);
        return product;
    }
    public Product UpdateProduct(int productId, string name, decimal price, int quantity)
    {

        //validation
        if (productId < 0) throw new ArgumentOutOfRangeException($"{nameof(productId)} is zero");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException($"{nameof(name)} can not be null or empty.");
        if (price <= 0) throw new ArgumentOutOfRangeException($"{nameof(price)} can not be zero or less.");
        if ((quantity < 0)) throw new ArgumentOutOfRangeException($"{nameof(quantity)} can not be negative");

        var product = _dbContext.Products.FirstOrDefault(x => x.Id == productId);
        if (product is null)
            throw new KeyNotFoundException($"{nameof(product)} not found");

        product.Price = price;
        product.Name = name;
        product.StockQuantity = quantity;

        return product;
    }
    public Product DeleteProduct(int productId)
    {
        if (productId < 0) throw new ArgumentOutOfRangeException($"{nameof(productId)} is zero");

        var product = _dbContext.Products.FirstOrDefault(x => x.Id == productId);
        if (product is null)
            throw new KeyNotFoundException($"{nameof(product)} not found");

        _dbContext.Products.Remove(product);

        return product;

    }
    public List<Product> GetAllProduct() { return _dbContext.Products; }

    public Product GetProductById(int productId)
    {
        var product = _dbContext.Products.FirstOrDefault(x => x.Id == productId);
        if (product is null)
            throw new KeyNotFoundException($"{nameof(product)} not found");

        return product;
    }
}
