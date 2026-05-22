using Step02.DAL.Data;
using Step02.DAL.Entities;
using Step02.DAL.Repositorties;

namespace Step02.DAL.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DatabaseContext _dbContext;

    public ProductRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Product product)
    {
        //- Validation
        if (string.IsNullOrWhiteSpace(product.Name)) throw new ArgumentNullException("name");
        if (product.Price <= 0) throw new ArgumentOutOfRangeException("Price can not be or less zero.");
        if (product.StockQuantity < 0) throw new ArgumentOutOfRangeException("Quantity can not be negative.");

        product.Id = DatabaseContext.ProductNextId++;
        _dbContext.Products.Add(product);
    }


    public void Update(Product product)
    {

        //validation
        if (product.Id < 0) throw new ArgumentOutOfRangeException($"{nameof(product.Id)} is zero");
        if (string.IsNullOrWhiteSpace(product.Name)) throw new ArgumentNullException($"{nameof(product.Name)} can not be null or empty.");
        if (product.Price <= 0) throw new ArgumentOutOfRangeException($"{nameof(product.Price)} can not be zero or less.");
        if (product.StockQuantity < 0) throw new ArgumentOutOfRangeException($"{nameof(product.StockQuantity)} can not be negative");

        var productForUpdate = GetById(product.Id);

        //- Update
        productForUpdate.Price = product.Price;
        productForUpdate.Name = product.Name;
        productForUpdate.StockQuantity = product.StockQuantity;
    }

    public void Delete(int productId)
    {
        var product = GetById(productId);

        _dbContext.Products.Remove(product);

    }

    public List<Product> GetAll() { return _dbContext.Products; }


    public Product GetById(int id)
    {
        if (id < 0) throw new ArgumentOutOfRangeException($"{nameof(id)} is zero");

        var product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
        if (product is null)
            throw new KeyNotFoundException($"{nameof(product)} not found");

        return product;
    }
}